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

bool IsStandardLayoutClass(SchemaClassInfoData_t* classData) {
    {
        auto pClass = classData;
        int classesWithFields = 0;
        do {
            classesWithFields += ((pClass->m_nSize > 1) || (pClass->m_nFieldCount != 0)) ? 1 : 0;

            if (classesWithFields > 1) return false;

            pClass = (pClass->m_pBaseClasses == nullptr) ? nullptr : pClass->m_pBaseClasses->m_pClass;
        } while (pClass != nullptr);
    }

    auto fields = classData->m_pFields;
    auto fieldsCount = classData->m_nFieldCount;
    for (uint16_t i = 0; i < fieldsCount; i++) {
        auto fieldType = fields[i].m_pType;
        if (fieldType->m_eTypeCategory == SchemaTypeCategory_t::SCHEMA_TYPE_DECLARED_CLASS) {
            CSchemaType_DeclaredClass* fClass = reinterpret_cast<CSchemaType_DeclaredClass*>(fieldType);
            if (fClass->m_pClassInfo && !IsStandardLayoutClass(fClass->m_pClassInfo)) return false;
        }
    }

    return true;
}

std::string chainEntityIndex = "__m_pChainEntity";

void FindChainer(int& chainer_offset, CSchemaClassInfo* classInfo)
{
    for (int i = 0; i < classInfo->m_nBaseClassCount; i++)
    {
        auto baseClass = classInfo->m_pBaseClasses[i].m_pClass;
        if (baseClass)
        {
            for (int j = 0; j < baseClass->m_nFieldCount; j++)
            {
                if (baseClass->m_pFields[j].m_pszName == chainEntityIndex)
                {
                    chainer_offset = baseClass->m_pFields[j].m_nSingleInheritanceOffset;
                    break;
                }
            }
        }
        if (chainer_offset != -1) break;
    }

    if (chainer_offset == -1)
    {
        for (int i = 0; i < classInfo->m_nBaseClassCount; i++)
        {
            auto baseClass = classInfo->m_pBaseClasses[i].m_pClass;
            if (baseClass)
            {
                FindChainer(chainer_offset, baseClass);
                if (chainer_offset != -1) break;
            }
        }
    }
}

void ReadClasses(CSchemaType_DeclaredClass* declClass)
{
    auto classInfo = declClass->m_pClassInfo;

    if (!classInfo) return;

    uint64_t class_hash = (uint64_t)(hash_32_fnv1a_const(classInfo->m_pszName)) << 32;
    auto field_size = classInfo->m_nFieldCount;
    auto fields = classInfo->m_pFields;
    bool isStruct = IsStandardLayoutClass(classInfo);
    void* classVtable = nullptr;
    bool hasVtable = g_pS2BinLib->FindVtable("server", classInfo->m_pszName, &classVtable) == 0;

    int chainer_offset = -1;

    for (int i = 0; i < field_size; i++)
    {
        if (fields[i].m_pszName == chainEntityIndex)
        {
            chainer_offset = fields[i].m_nSingleInheritanceOffset;
            break;
        }
    }

    if (chainer_offset == -1)
    {
        FindChainer(chainer_offset, classInfo);
    }

    for (int i = 0; i < field_size; i++)
    {
        auto field = fields[i];

        void* vtable = nullptr;
        int stateChangedIndex = -1;
        uint64_t fieldHash = (class_hash | hash_32_fnv1a_const(field.m_pszName));

        std::string networkVarName = std::string("NetworkVar_") + field.m_pszName;
        int result = g_pS2BinLib->FindVtableNested2("server", classInfo->m_pszName, networkVarName.c_str(), &vtable);
        if (result == 0) {
            uint64_t index;
            result = g_pS2BinLib->FindNetworkvarVtableStatechanged((uint64_t)vtable, &index);
            if (result == 0) {
                stateChangedIndex = (int)index;
                networkVarStateChangedOffsets.emplace(vtable, stateChangedIndex);
            }
        }

        offsets.insert({ fieldHash, { chainer_offset, stateChangedIndex, (uint32_t)field.m_nSingleInheritanceOffset, isStruct, hasVtable } });
    }
}
