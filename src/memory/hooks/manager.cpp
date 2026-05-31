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

#include "manager.h"

#include <map>
#include <vector>

#include <api/shared/string.h>

IFunctionHook* HooksManager::CreateFunctionHook()
{
    return new FunctionHook();
}

IVFunctionHook* HooksManager::CreateVFunctionHook()
{
    return new VFunctionHook();
}

IMFunctionHook* HooksManager::CreateMFunctionHook()
{
    return new MFunctionHook();
}

void HooksManager::DestroyFunctionHook(IFunctionHook* hook)
{
    delete (FunctionHook*)hook;
}

void HooksManager::DestroyVFunctionHook(IVFunctionHook* hook)
{
    delete (VFunctionHook*)hook;
}

void HooksManager::DestroyMFunctionHook(IMFunctionHook* hook)
{
    delete (MFunctionHook*)hook;
}

void HooksManager::Initialize()
{
}

void HooksManager::Shutdown()
{
}
