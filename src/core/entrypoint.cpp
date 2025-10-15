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

#include "managed/host/dynlib.h"
#include "managed/host/strconv.h"

#include <engine/entities/listener.h>
#include <engine/entities/entitysystem.h>
#include <engine/fixes/entrypoint.h>
#include <engine/gamesystem/gamesystem.h>

#include <public/tier0/icommandline.h>
#include <public/tier1/utlstringtoken.h>

#include <api/shared/plat.h>
#include <api/shared/string.h>
#include <api/shared/files.h>

#include <fmt/format.h>

#include <s2binlib/s2binlib.h>

SwiftlyCore g_SwiftlyCore;
InterfacesManager g_ifaceService;
CSteamGameServerAPIContext g_SteamAPI;

IVFunctionHook* g_pGameServerSteamAPIActivated = nullptr;
IVFunctionHook* g_pGameServerSteamAPIDeactivated = nullptr;

void GameServerSteamAPIActivatedHook(void* _this);
void GameServerSteamAPIDeactivatedHook(void* _this);

bool SwiftlyCore::Load(BridgeKind_t kind)
{
    m_iKind = kind;
    SetupConsoleColors();

    s2binlib_initialize(Plat_GetGameDirectory(), "csgo");
    s2binlib_set_module_base_from_pointer("server", g_ifaceService.FetchInterface<IServerGameDLL>(INTERFACEVERSION_SERVERGAMEDLL));
    s2binlib_set_module_base_from_pointer("engine2", g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER));

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

    auto consoleoutput = g_ifaceService.FetchInterface<IConsoleOutput>(CONSOLEOUTPUT_INTERFACE_VERSION);
    consoleoutput->Initialize();
    if (bool* b = std::get_if<bool>(&configuration->GetValue("core.ConsoleFilter")))
        if (*b) consoleoutput->ToggleFilter();

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

    g_pGameServerSteamAPIActivated = hooksmanager->CreateVFunctionHook();
    g_pGameServerSteamAPIActivated->SetHookFunction(INTERFACEVERSION_SERVERGAMEDLL, gamedata->GetOffsets()->Fetch("IServerGameDLL::GameServerSteamAPIActivated"), (void*)GameServerSteamAPIActivatedHook);
    g_pGameServerSteamAPIActivated->Enable();

    g_pGameServerSteamAPIDeactivated = hooksmanager->CreateVFunctionHook();
    g_pGameServerSteamAPIDeactivated->SetHookFunction(INTERFACEVERSION_SERVERGAMEDLL, gamedata->GetOffsets()->Fetch("IServerGameDLL::GameServerSteamAPIDeactivated"), (void*)GameServerSteamAPIDeactivatedHook);
    g_pGameServerSteamAPIDeactivated->Enable();

    StartFixes();

    auto scripting = g_ifaceService.FetchInterface<IScriptingAPI>(SCRIPTING_INTERFACE_VERSION);

    if (!InitializeHostFXR(std::string(Plat_GetGameDirectory()) + "/csgo/" + m_sCorePath))
    {
        auto crashreporter = g_ifaceService.FetchInterface<ICrashReporter>(CRASHREPORTER_INTERFACE_VERSION);
        crashreporter->ReportPreventionIncident("Managed", fmt::format("Couldn't initialize the .NET runtime. Make sure you installed `swiftlys2-{}-{}-with-runtimes.zip`.", WIN_LINUX("windows", "linux"), GetVersion()));
        return true;
    }
    if (!InitializeDotNetAPI(scripting->GetNativeFunctions(), scripting->GetNativeFunctionsCount()))
    {
        auto crashreporter = g_ifaceService.FetchInterface<ICrashReporter>(CRASHREPORTER_INTERFACE_VERSION);
        crashreporter->ReportPreventionIncident("Managed", "Couldn't initialize the .NET scripting API.");
        return true;
    }

    return true;
}

bool SwiftlyCore::Unload()
{
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

    if (g_pGameServerSteamAPIActivated != nullptr)
    {
        g_pGameServerSteamAPIActivated->Disable();
        hooksmanager->DestroyVFunctionHook(g_pGameServerSteamAPIActivated);
        g_pGameServerSteamAPIActivated = nullptr;
    }

    if (g_pGameServerSteamAPIDeactivated != nullptr)
    {
        g_pGameServerSteamAPIDeactivated->Disable();
        hooksmanager->DestroyVFunctionHook(g_pGameServerSteamAPIDeactivated);
        g_pGameServerSteamAPIDeactivated = nullptr;
    }

    StopFixes();

    ShutdownGameSystem();

    return true;
}

void GameServerSteamAPIActivatedHook(void* _this)
{
    if (!CommandLine()->HasParm("-dedicated") || g_SteamAPI.SteamUGC())
        return;

    g_SteamAPI.Init();
    static auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    playermanager->SteamAPIServerActivated();

    return reinterpret_cast<decltype(&GameServerSteamAPIActivatedHook)>(g_pGameServerSteamAPIActivated->GetOriginal())(_this);
}

void GameServerSteamAPIDeactivatedHook(void* _this)
{
    return reinterpret_cast<decltype(&GameServerSteamAPIDeactivatedHook)>(g_pGameServerSteamAPIDeactivated->GetOriginal())(_this);
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

std::map<std::string, void*> g_mInterfacesCache;

void* SwiftlyCore::GetInterface(const std::string& interface_name)
{
    auto it = g_mInterfacesCache.find(interface_name);
    if (it != g_mInterfacesCache.end())
        return it->second;

    void* ifaceptr = nullptr;
    void* ifaceCreate = nullptr;
    if (INTERFACEVERSION_SERVERGAMEDLL == interface_name || INTERFACEVERSION_SERVERGAMECLIENTS == interface_name || SOURCE2GAMEENTITIES_INTERFACE_VERSION == interface_name) {
        void* lib = load_library(
            (const char_t*)WIN_LINUX(
                StringWide((Plat_GetGameDirectory() + std::string("\\csgo\\bin\\win64\\server.dll"))).c_str(),
                (Plat_GetGameDirectory() + std::string("/csgo/bin/linuxsteamrt64/libserver.so")).c_str()
            )
        );
        ifaceCreate = get_export(lib, "CreateInterface");
        unload_library(lib);
    }
    else if (SCHEMASYSTEM_INTERFACE_VERSION == interface_name) {
        void* lib = load_library(
            (const char_t*)WIN_LINUX(
                StringWide(Plat_GetGameDirectory() + std::string("\\bin\\win64\\schemasystem.dll")).c_str(),
                (Plat_GetGameDirectory() + std::string("/bin/linuxsteamrt64/libschemasystem.so")).c_str()
            )
        );
        ifaceCreate = get_export(lib, "CreateInterface");
        unload_library(lib);
    }
    else {
        void* lib = load_library(
            (const char_t*)WIN_LINUX(
                StringWide(Plat_GetGameDirectory() + std::string("\\bin\\win64\\engine2.dll")).c_str(),
                (Plat_GetGameDirectory() + std::string("/bin/linuxsteamrt64/libengine2.so")).c_str()
            )
        );
        ifaceCreate = get_export(lib, "CreateInterface");
        unload_library(lib);
    }

    if (ifaceCreate != nullptr)
    {
        ifaceptr = reinterpret_cast<void* (*)(const char*, int*)>(ifaceCreate)(interface_name.c_str(), nullptr);
    }

    if (ifaceptr != nullptr) g_mInterfacesCache.insert({ interface_name, ifaceptr });

    return ifaceptr;
}

void SwiftlyCore::SendConsoleMessage(const std::string& message)
{
    Msg("%s", message.c_str());
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