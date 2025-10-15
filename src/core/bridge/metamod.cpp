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

#include "metamod.h"
#include "../entrypoint.h"

#include <api/interfaces/manager.h>
#include <memory/gamedata/manager.h>

#include <public/iserver.h>
#include <public/tier0/icommandline.h>
#include <steam/steam_gameserver.h>
#include <public/engine/igameeventsystem.h>

#include <s2binlib/s2binlib.h>

SwiftlyMMBridge g_MMPluginBridge;

class GameSessionConfiguration_t
{
};

SH_DECL_HOOK2(IGameEventManager2, FireEvent, SH_NOATTRIB, 0, bool, IGameEvent*, bool);
SH_DECL_HOOK2(IGameEventManager2, LoadEventsFromFile, SH_NOATTRIB, 0, int, const char*, bool);
SH_DECL_HOOK2(CServerSideClientBase, FilterMessage, SH_NOATTRIB, 0, bool, const CNetMessage*, INetChannel*);
SH_DECL_HOOK2_void(IServerGameClients, ClientCommand, SH_NOATTRIB, 0, CPlayerSlot, const CCommand&);
SH_DECL_HOOK3(IVEngineServer2, SetClientListening, SH_NOATTRIB, 0, bool, CPlayerSlot, CPlayerSlot, bool);
SH_DECL_HOOK3_void(INetworkServerService, StartupServer, SH_NOATTRIB, 0, const GameSessionConfiguration_t&, ISource2WorldSession*, const char*);
SH_DECL_HOOK4_void(IServerGameClients, ClientPutInServer, SH_NOATTRIB, 0, CPlayerSlot, char const*, int, uint64);
SH_DECL_HOOK5_void(IServerGameClients, ClientDisconnect, SH_NOATTRIB, 0, CPlayerSlot, ENetworkDisconnectionReason, const char*, uint64, const char*);
SH_DECL_HOOK6(IServerGameClients, ClientConnect, SH_NOATTRIB, 0, bool, CPlayerSlot, const char*, uint64, const char*, bool, CBufferString*);
SH_DECL_HOOK6_void(IServerGameClients, OnClientConnected, SH_NOATTRIB, 0, CPlayerSlot, const char*, uint64, const char*, const char*, bool);
SH_DECL_HOOK7_void(ISource2GameEntities, CheckTransmit, SH_NOATTRIB, 0, CCheckTransmitInfo**, int, CBitVec<16384>&, CBitVec<16384>&, const Entity2Networkable_t**, const uint16_t*, int);

ICvar* g_pcVar = nullptr;

PLUGIN_EXPOSE(SwiftlyMMBridge, g_MMPluginBridge);
bool SwiftlyMMBridge::Load(PluginId id, ISmmAPI* ismm, char* error, size_t maxlen, bool late)
{
    PLUGIN_SAVEVARS();
    g_SMAPI->AddListener(this, this);

    GET_V_IFACE_CURRENT(GetEngineFactory, g_pCVar, ICvar, CVAR_INTERFACE_VERSION);

    META_CONVAR_REGISTER(FCVAR_RELEASE | FCVAR_SERVER_CAN_EXECUTE | FCVAR_CLIENT_CAN_EXECUTE | FCVAR_GAMEDLL);

    s2binlib_initialize(Plat_GetGameDirectory(), "csgo");
    s2binlib_set_module_base_from_pointer("server", g_ifaceService.FetchInterface<IServerGameDLL>(INTERFACEVERSION_SERVERGAMEDLL));
    s2binlib_set_module_base_from_pointer("engine2", g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER));

    bool result = g_SwiftlyCore.Load(BridgeKind_t::Metamod);

    if (late)
    {
        g_SteamAPI.Init();
        static auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
        playermanager->SteamAPIServerActivated();
    }

    return result;
}

bool SwiftlyMMBridge::Unload(char* error, size_t maxlen)
{
    return g_SwiftlyCore.Unload();
}

void SwiftlyMMBridge::AllPluginsLoaded()
{

}

void SwiftlyMMBridge::OnLevelInit(char const* pMapName, char const* pMapEntities, char const* pOldLevel, char const* pLandmarkName, bool loadGame, bool background)
{
    g_SwiftlyCore.OnMapLoad(pMapName);
}

void SwiftlyMMBridge::OnLevelShutdown()
{
    g_SwiftlyCore.OnMapUnload();
}

std::map<std::string, void*> g_mInterfacesCache;

void* SwiftlyMMBridge::GetInterface(const std::string& interface_name)
{
    auto it = g_mInterfacesCache.find(interface_name);
    if (it != g_mInterfacesCache.end())
        return it->second;

    void* ifaceptr = nullptr;
    if (interface_name == FILESYSTEM_INTERFACE_VERSION)
        ifaceptr = g_SMAPI->VInterfaceMatch(g_SMAPI->GetFileSystemFactory(), FILESYSTEM_INTERFACE_VERSION, 0);
    else if (INTERFACEVERSION_SERVERGAMEDLL == interface_name || INTERFACEVERSION_SERVERGAMECLIENTS == interface_name || SOURCE2GAMEENTITIES_INTERFACE_VERSION == interface_name)
        ifaceptr = g_SMAPI->VInterfaceMatch(g_SMAPI->GetServerFactory(), interface_name.c_str(), 0);
    else
        ifaceptr = g_SMAPI->VInterfaceMatch(g_SMAPI->GetEngineFactory(), interface_name.c_str(), 0);

    if (ifaceptr) g_mInterfacesCache.insert({ interface_name, ifaceptr });

    return ifaceptr;
}

const char* SwiftlyMMBridge::GetAuthor()
{
    return "Swiftly Development Team";
}

const char* SwiftlyMMBridge::GetName()
{
    return "SwiftlyS2";
}

const char* SwiftlyMMBridge::GetDescription()
{
    return "C# Framework for Source2-based games";
}

const char* SwiftlyMMBridge::GetURL()
{
    return "https://github.com/swiftly-solution/swiftly";
}

const char* SwiftlyMMBridge::GetLicense()
{
    return "GNU GPLv3";
}

const char* SwiftlyMMBridge::GetVersion()
{
#ifndef SWIFTLY_VERSION
    return "Local";
#else
    return SWIFTLY_VERSION;
#endif
}

const char* SwiftlyMMBridge::GetDate()
{
    return __DATE__;
}

const char* SwiftlyMMBridge::GetLogTag()
{
    return "SwiftlyS2";
}