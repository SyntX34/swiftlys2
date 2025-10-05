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
    if (s2binlib_pattern_scan(library.c_str(), pattern.c_str(), &result) == 0) {
        return reinterpret_cast<void*>(result);
    }
    else {
        return nullptr;
    }
    
}