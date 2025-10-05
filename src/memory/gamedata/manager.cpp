/************************************************************************************************
 *  SwiftlyS2 is a scripting framework for Source2-based games.
 *  Copyright (C) 2025 Swiftly Solution SRL via Sava Andrei-Sebastian and it's contributors
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 ************************************************************************************************/

#include "manager.h"

#include "offsets.h"
#include "patches.h"
#include "signatures.h"

#include <public/eiface.h>
#include <api/interfaces/manager.h>
#include <api/shared/string.h>
#include <api/shared/plat.h>
#include <api/shared/files.h>
#include "rva.h"
#include <fmt/format.h>
#include <s2binlib/s2binlib.h>
#include <emmintrin.h>
#ifdef _WIN32
#include <intrin.h>
#define ctz(x) _tzcnt_u32(x)
#else
#define ctz(x) __builtin_ctz(x)
#endif

IGameDataOffsets* GameDataManager::GetOffsets()
{
    if (m_pOffsets == nullptr) m_pOffsets = new GameDataOffsets();
    return m_pOffsets;
}

IGameDataSignatures* GameDataManager::GetSignatures()
{
    if (m_pSignatures == nullptr) m_pSignatures = new GameDataSignatures();
    return m_pSignatures;
}

IGameDataPatches* GameDataManager::GetPatches()
{
    if (m_pPatches == nullptr) m_pPatches = new GameDataPatches();
    return m_pPatches;
}

DynLibUtils::CModule DetermineModuleByLibrary(std::string library) {
    if (library == "server")
        return DynLibUtils::CModule(g_ifaceService.FetchInterface<IServerGameDLL>(INTERFACEVERSION_SERVERGAMEDLL));
    else if (library == "engine2")
        return DynLibUtils::CModule(g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER));
    else
        return DynLibUtils::CModule(library);
}

extern std::vector<uint8_t> HexToByte(const char* src, uint64_t& length);

std::map<std::string, uint8_t*> m_binaries;
std::map<std::string, void*> m_handles;
std::map<std::string, int> m_binariesSize;

void* FindSignature(std::string library, std::string pattern)
{
    uint64_t result;
    s2binlib_pattern_scan(library.c_str(), pattern.c_str(), &result);
    printf("Rust found signature %llx\n", result);

    if (m_binaries.find(library) == m_binaries.end()) {
        std::string path;
        if (library == "server" || library == "client" || library == "matchmaking")
            path = GeneratePath(std::string("bin") + WIN_LINUX("\\", "/") + WIN_LINUX("win64", "linuxsteamrt64") + WIN_LINUX("\\", "/lib") + library + WIN_LINUX(".dll", ".so"));
        else
            path = fmt::format("{}{}{}{}{}{}{}{}", Plat_GetGameDirectory(), WIN_LINUX("\\", "/"), "bin", WIN_LINUX("\\", "/"), WIN_LINUX("win64", "linuxsteamrt64"), WIN_LINUX("\\", "/lib"), library, WIN_LINUX(".dll", ".so"));
        
        auto fp = std::fopen(path.c_str(), "rb");
        std::fseek(fp, 0u, SEEK_END);
        auto size = std::ftell(fp);
        m_binariesSize[library] = size;
        std::fseek(fp, 0u, SEEK_SET);
        auto buffer = new uint8_t[size];
        std::fread(buffer, 1u, size, fp);
        std::fclose(fp);
        
        m_binaries[library] = buffer;

        m_handles[library] = LoadBinaryModule(path.c_str());
    }
    else {
        return nullptr;
    }

    uint64_t length = 0;
    auto patterns = HexToByte(("\\x" + replace(replace(pattern, "?", "2A"), " ", "\\x")).c_str(), length);
    auto binBytes = m_binaries[library];
    auto binSize = m_binariesSize[library];
    auto libModule = DetermineModuleByLibrary(library);

    auto first = _mm_set1_epi8(patterns[0]);
    for (int i = 0; i <= binSize - length - 16; i += 16) {
        auto data = _mm_loadu_si128((__m128i*)(binBytes + i));
        auto cmp = _mm_cmpeq_epi8(data, first);
        int mask = _mm_movemask_epi8(cmp);

        while (mask) {
            int pos = ctz(mask);
            int idx = i + pos;
            bool match = true;
            for (int j = 1; j < length; j++) {
                if (patterns[j] != 0x2A && binBytes[idx + j] != patterns[j]) {
                    match = false;
                    break;
                }
            }
            if (match) {
                auto addr = ConvertFileOffsetToRVA(m_handles[library], idx);
                printf("Found signature %llx\n", libModule.GetModuleBase().Offset(addr));
                return libModule.GetModuleBase().Offset(addr).RCast<void*>();
            }
            mask &= mask - 1;
        }
    }

    return nullptr;
}