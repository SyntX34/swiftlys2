/************************************************************************************************
 *  SwiftlyS2 is a scripting framework for Source2-based games.
 *  Copyright (C) 2023-2026 Swiftly Solution SRL via Sava Andrei-Sebastian and it's contributors
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

#include "schema.h"

#include <fmt/format.h>
#include <unordered_map>
#include <unordered_set>
#include <string>

#include <core/entrypoint.h>

#include <api/interfaces/interfaces.h>
#include <api/shared/files.h>
#include <api/shared/plat.h>
#include <api/memory/virtual/call.h>    

#include <public/entity2/entityclass.h>

#define CBaseEntity_m_nSubclassID 0x9DC483B8C02CE796

std::unordered_map<uint64_t, SchemaField, FNV1aHasher64> offsets;
std::unordered_map<void*, int32_t> networkVarStateChangedOffsets;

class CNetworkVarChainer
{
public:
    CEntityInstance* m_pEntity;

private:
    uint8 pad_0000[24];

public:
    ChangeAccessorFieldPathIndex_t m_PathIndex;

private:
    uint8 pad_0024[4];
};

void CSDKSchema::Load()
{
    g_pLogger->Info("SDK", "Loading SDK classes and network var vtables...\n");

    offsets.clear();
    networkVarStateChangedOffsets.clear();

    auto gts = g_pGameSchemaSystem->GlobalTypeScope();

    int classes_count = gts->m_DeclaredClasses.m_Map.Count();

    FOR_EACH_MAP(gts->m_DeclaredClasses.m_Map, iter)
    {
        ReadClasses(gts->m_DeclaredClasses.m_Map.Element(iter));
    }

    for (int i = 0; i < g_pGameSchemaSystem->m_TypeScopes.GetNumStrings(); i++)
    {
        auto ts = g_pGameSchemaSystem->m_TypeScopes[i];

        classes_count += ts->m_DeclaredClasses.m_Map.Count();

        FOR_EACH_MAP(ts->m_DeclaredClasses.m_Map, iter)
        {
            ReadClasses(ts->m_DeclaredClasses.m_Map.Element(iter));
        }
    }

    g_pLogger->Info("SDK", fmt::format("Finished loading {} SDK classes ({} fields).\n", classes_count, offsets.size()));

    int networkVarFields = 0;
    for (auto& [offset_hash, offset_data] : offsets)
        if (offset_data.m_nStateChangedOffset != -1)
            networkVarFields++;

    g_pLogger->Info("SDK", fmt::format("Loaded {} network var fields.\n", networkVarFields));

    g_pGameSchemaSystem->PrintSchemaStats("");
}

void CSDKSchema::SetStateChanged(void* pEntity, uint64_t uHash)
{
    if (pEntity == nullptr) return;

    auto fieldData = offsets.find(uHash);
    if (fieldData == offsets.end()) return;

    auto& fieldInfo = fieldData->second;

    if (fieldInfo.m_nStateChangedOffset != -1) {
        // This slot belongs to the embedded field's vtable. Its thunk adds the
        // field offset when forwarding the notification to its enclosing object.
        auto networkVar = reinterpret_cast<NetworkVar*>((uintptr_t)pEntity + fieldInfo.m_uOffset);
        networkVar->StateChanged(fieldInfo.m_nStateChangedOffset, NetworkStateChangedData(uint32_t(0)));
        return;
    }

    if (fieldInfo.m_bHasVtable) {
        // A nested schema handle can itself point at a NetworkVar subclass,
        // e.g. fogparams_t inside CFogController. Use its actual runtime vtable.
        auto stateChanged = networkVarStateChangedOffsets.find(*reinterpret_cast<void**>(pEntity));
        if (stateChanged != networkVarStateChangedOffsets.end()) {
            reinterpret_cast<NetworkVar*>(pEntity)->StateChanged(stateChanged->second, NetworkStateChangedData(fieldInfo.m_uOffset));
            return;
        }
    }

    if (fieldInfo.m_nChainerOffset != -1) {
        CNetworkVarChainer* pChainer = (CNetworkVarChainer*)((uintptr_t)pEntity + fieldInfo.m_nChainerOffset);

        CEntityInstance* pEntity = pChainer->m_pEntity;
        if (pEntity != nullptr)
            pEntity->NetworkStateChanged(NetworkStateChangedData(fieldInfo.m_uOffset, -1, pChainer->m_PathIndex));
    } else if (!fieldInfo.m_bIsStruct) {
        reinterpret_cast<CEntityInstance*>(pEntity)->NetworkStateChanged(NetworkStateChangedData(fieldInfo.m_uOffset));
    }
}

int32_t CSDKSchema::GetOffset(const char* sClassName, const char* sMemberName)
{
    uint32_t class_hash = hash_32_fnv1a_const(sClassName);
    uint64_t fieldHash = ((uint64_t)(class_hash) << 32 | hash_32_fnv1a_const(sMemberName));
    return GetOffset(fieldHash);
}

int32_t CSDKSchema::GetOffset(uint64_t uHash)
{
    auto it = offsets.find(uHash);
    if (it == offsets.end()) return -1;
    else return it->second.m_uOffset;
}

void* CSDKSchema::GetPropPtr(void* pEntity, const char* sClassName, const char* sMemberName)
{
    uint32_t class_hash = hash_32_fnv1a_const(sClassName);
    uint64_t fieldHash = ((uint64_t)(class_hash) << 32 | hash_32_fnv1a_const(sMemberName));

    return GetPropPtr(pEntity, fieldHash);
}

void* CSDKSchema::GetPropPtr(void* pEntity, uint64_t uHash)
{
    auto it = offsets.find(uHash);
    if (it == offsets.end()) return nullptr;

    auto& fieldInfo = it->second;
    return reinterpret_cast<void*>((uintptr_t)pEntity + fieldInfo.m_uOffset);
}

void* CSDKSchema::GetVData(void* pEntity)
{
    void* subclassPtr = GetPropPtr(pEntity, CBaseEntity_m_nSubclassID);
    return *(void**)((uintptr_t)subclassPtr + 4);
}

void* CSDKSchema::GetDatamapFunction(const char* className, const char* functionName)
{
    auto entitySystem = g_pEntSystem->GetEntitySystem();

    auto entityClassIndex = entitySystem->m_entClassesByCPPClassname.Find(className);
    if (!entitySystem->m_entClassesByCPPClassname.IsValidIndex(entityClassIndex)) return nullptr;

    auto entityClass = entitySystem->m_entClassesByCPPClassname[entityClassIndex];
    return reinterpret_cast<void*>(entityClass->m_NameToThinkFunc(functionName));
}
