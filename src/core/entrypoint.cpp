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

#include "entrypoint.h"
#include "bridge/metamod.h"
#include <api/interfaces/manager.h>
#include <api/interfaces/interfaces.h>
#include <api/scripting/scripting.h>    
#include <api/memory/hooks/manager.h>
#include "console/colors.h"
#include <core/managed/host/host.h>

#include <engine/entities/listener.h>
#include <engine/entities/entitysystem.h>
#include <engine/gamesystem/gamesystem.h>

#include <public/tier0/icommandline.h>
#include <public/tier1/utlstringtoken.h>

#include <api/shared/plat.h>
#include <api/shared/string.h>

#include <fmt/format.h>

#include <s2binlib/s2binlib.h>

SwiftlyCore g_SwiftlyCore;
InterfacesManager g_ifaceService;

bool SwiftlyCore::Load(BridgeKind_t kind)
{
    m_iKind = kind;
    SetupConsoleColors();

    s2binlib_initialize(Plat_GetGameDirectory(), "csgo");
    s2binlib_set_module_base_from_pointer("server", (uint64_t)g_ifaceService.FetchInterface<IServerGameDLL>(INTERFACEVERSION_SERVERGAMEDLL));
    s2binlib_set_module_base_from_pointer("engine2", (uint64_t)g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER));

    auto logger = g_ifaceService.FetchInterface<ILogger>(LOGGER_INTERFACE_VERSION);

    if (GetCurrentGame() == "unknown") {
        auto engine = g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER);

        if (engine) logger->Error("Entrypoint", fmt::format("Unknown game detected. App ID: {}", engine->GetAppID()));
        else logger->Error("Entrypoint", "Unknown game detected. No engine interface available.");

        return false;
    }

    m_sCorePath = CommandLine()->ParmValue(CUtlStringToken("-sw_path"), WIN_LINUX("addons\\swiftlys2", "addons/swiftlys2"));
    if (!ends_with(m_sCorePath, WIN_LINUX("\\", "/"))) m_sCorePath += WIN_LINUX("\\", "/");

    auto configuration = g_ifaceService.FetchInterface<IConfiguration>(CONFIGURATION_INTERFACE_VERSION);
    configuration->InitializeExamples();

    if (!configuration->Load()) {
        logger->Error("Entrypoint", "Couldn't load the core configuration.");
        return false;
    }

    auto sdkclass = g_ifaceService.FetchInterface<ISDKSchema>(SDKSCHEMA_INTERFACE_VERSION);
    sdkclass->Load();

    auto gamedata = g_ifaceService.FetchInterface<IGameDataManager>(GAMEDATA_INTERFACE_VERSION);

    gamedata->GetOffsets()->Load(GetCurrentGame());
    gamedata->GetSignatures()->Load(GetCurrentGame());
    gamedata->GetPatches()->Load(GetCurrentGame());

    if (std::string* s = std::get_if<std::string>(&configuration->GetValue("core.PatchesToPerform"))) {
        auto patches = explodeToSet(*s, " ");
        for (const auto& patch : patches) {
            if (gamedata->GetPatches()->Exists(patch)) {
                gamedata->GetPatches()->Apply(patch);
                logger->Info("Patching", fmt::format("Applied patch: {}", patch));
            }
            else {
                logger->Warning("Patching", fmt::format("Couldn't find patch: {}", patch));
            }
        }
    }

    auto entsystem = g_ifaceService.FetchInterface<IEntitySystem>(ENTITYSYSTEM_INTERFACE_VERSION);
    entsystem->Initialize();

    auto cvarmanager = g_ifaceService.FetchInterface<IConvarManager>(CONVARMANAGER_INTERFACE_VERSION);
    cvarmanager->Initialize();

    auto evmanager = g_ifaceService.FetchInterface<IEventManager>(GAMEEVENTMANAGER_INTERFACE_VERSION);
    evmanager->Initialize(GetCurrentGame());

    if (!InitGameSystem()) {
        logger->Error("Game System", "Couldn't initialize the Game System.\n");
        return false;
    }

    auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    playermanager->Initialize();

    auto databasemanager = g_ifaceService.FetchInterface<IDatabaseManager>(DATABASEMANAGER_INTERFACE_VERSION);
    databasemanager->Initialize();

    auto translations = g_ifaceService.FetchInterface<ITranslations>(TRANSLATIONS_INTERFACE_VERSION);
    translations->Initialize();

    auto netmessages = g_ifaceService.FetchInterface<INetMessages>(NETMESSAGES_INTERFACE_VERSION);
    netmessages->Initialize();

    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    servercommands->Initialize();

    auto hooksmanager = g_ifaceService.FetchInterface<IHooksManager>(HOOKSMANAGER_INTERFACE_VERSION);
    hooksmanager->Initialize();

    IExtensionManager* extManager = g_ifaceService.FetchInterface<IExtensionManager>(EXTENSIONMANAGER_INTERFACE_VERSION);
    extManager->Load();

    auto scripting = g_ifaceService.FetchInterface<IScriptingAPI>(SCRIPTING_INTERFACE_VERSION);

    InitializeHostFXR(std::string(Plat_GetGameDirectory()) + "/csgo/" + m_sCorePath);
    InitializeDotNetAPI(scripting->GetNativeFunctions(), scripting->GetNativeFunctionsCount());

    return true;
}

bool SwiftlyCore::Unload()
{
    IExtensionManager* extManager = g_ifaceService.FetchInterface<IExtensionManager>(EXTENSIONMANAGER_INTERFACE_VERSION);
    extManager->Unload();

    auto hooksmanager = g_ifaceService.FetchInterface<IHooksManager>(HOOKSMANAGER_INTERFACE_VERSION);
    hooksmanager->Shutdown();

    auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    playermanager->Shutdown();

    auto entsystem = g_ifaceService.FetchInterface<IEntitySystem>(ENTITYSYSTEM_INTERFACE_VERSION);
    entsystem->Shutdown();

    auto cvarmanager = g_ifaceService.FetchInterface<IConvarManager>(CONVARMANAGER_INTERFACE_VERSION);
    cvarmanager->Shutdown();

    auto evmanager = g_ifaceService.FetchInterface<IEventManager>(GAMEEVENTMANAGER_INTERFACE_VERSION);
    evmanager->Shutdown();

    auto netmessages = g_ifaceService.FetchInterface<INetMessages>(NETMESSAGES_INTERFACE_VERSION);
    netmessages->Shutdown();

    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    servercommands->Shutdown();

    ShutdownGameSystem();

    return true;
}

std::string current_map = "";
extern void* g_pOnMapLoadCallback;
extern void* g_pOnMapUnloadCallback;

void SwiftlyCore::OnMapLoad(std::string map_name)
{
    current_map = map_name;

    if (g_pOnMapLoadCallback)
        reinterpret_cast<void(*)(const char*)>(g_pOnMapLoadCallback)(map_name.c_str());
}

void SwiftlyCore::OnMapUnload()
{
    if (g_pOnMapUnloadCallback)
        reinterpret_cast<void(*)(const char*)>(g_pOnMapUnloadCallback)(current_map.c_str());

    current_map = "";
}

void* SwiftlyCore::GetInterface(const std::string& iface_name)
{
    if (m_iKind == BridgeKind_t::Metamod) return g_MMPluginBridge.GetInterface(iface_name);
    else return nullptr;
}

void SwiftlyCore::SendConsoleMessage(const std::string& message)
{
    if (m_iKind == BridgeKind_t::Metamod) g_MMPluginBridge.SendConsoleMessage(message);
}

std::string SwiftlyCore::GetCurrentGame()
{
    auto engine = g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER);
    if (!engine) return "unknown";

    switch (engine->GetAppID()) {
    case 730: return "cs2";
    default: return "unknown";
    }
}

int SwiftlyCore::GetMaxGameClients()
{
    auto engine = g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER);
    if (!engine) return 0;

    switch (engine->GetAppID()) {
    case 730: return 64;
    default: return 0;
    }
}

std::string& SwiftlyCore::GetCorePath()
{
    return m_sCorePath;
}

std::string SwiftlyCore::GetVersion()
{
#ifndef SWIFTLY_VERSION
    return "Local";
#else
    return SWIFTLY_VERSION;
#endif
}