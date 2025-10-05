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

#include "gameevents.h"

#include <core/bridge/metamod.h>

#include <api/interfaces/manager.h>

#include <api/shared/files.h>
#include <api/shared/jsonc.h>
#include <api/shared/string.h>

#include <memory/gamedata/manager.h>

#include <public/iserver.h>
#include <public/filesystem.h>

#include <map>
#include <stack>
#include <list>
#include <nlohmann/json.hpp>

#include <fmt/format.h>

#include <s2binlib/s2binlib.h>

using json = nlohmann::json;

SH_DECL_EXTERN2(IGameEventManager2, FireEvent, SH_NOATTRIB, 0, bool, IGameEvent*, bool);
SH_DECL_EXTERN2(IGameEventManager2, LoadEventsFromFile, SH_NOATTRIB, 0, int, const char*, bool);
SH_DECL_EXTERN3_void(IServerGameDLL, GameFrame, SH_NOATTRIB, 0, bool, bool, bool);
SH_DECL_EXTERN3_void(INetworkServerService, StartupServer, SH_NOATTRIB, 0, const GameSessionConfiguration_t&, ISource2WorldSession*, const char*);

std::map<uint64_t, std::function<int(std::string, IGameEvent*, bool&)>> g_mEventListeners;
std::map<uint64_t, std::function<int(std::string, IGameEvent*, bool&)>> g_mPostEventListeners;

std::list<std::list<std::pair<int64_t, std::function<void()>>>::iterator> queueRemoveTimeouts;
std::list<std::pair<int64_t, std::function<void()>>> timeoutsArray;
bool processingTimeouts = false;

std::set<std::string> g_sDumpedFiles;
json dumpedEvents;

std::stack<IGameEvent*> g_sEventStack;
std::set<std::string> g_sEnqueueListenEvents;
bool g_bEventsLoaded = false;

int g_uLoadEventFromFileHookID = 0;

IGameEventManager2* g_gameEventManager = nullptr;

void CEventManager::Initialize(std::string game_name)
{
    DynLibUtils::CModule servermodule = DetermineModuleByLibrary("server");
    uint64_t CGameEventManagerVTable;
    s2binlib_find_vtable("server", "CGameEventManager", &CGameEventManagerVTable);

    printf("CGameEventManagerVTable: %llx\n", CGameEventManagerVTable);

    auto networkserverservice = g_ifaceService.FetchInterface<INetworkServerService>(NETWORKSERVERSERVICE_INTERFACE_VERSION);

    SH_ADD_HOOK_MEMFUNC(INetworkServerService, StartupServer, networkserverservice, this, &CEventManager::OnStartupServer, true);

    g_uLoadEventFromFileHookID = SH_ADD_DVPHOOK(IGameEventManager2, LoadEventsFromFile, (IGameEventManager2*)(void*)CGameEventManagerVTable, SH_MEMBER(this, &CEventManager::LoadEventsFromFile), false);

    auto server = g_ifaceService.FetchInterface<IServerGameDLL>(INTERFACEVERSION_SERVERGAMEDLL);

    SH_ADD_HOOK_MEMFUNC(IServerGameDLL, GameFrame, server, this, &CEventManager::GameFrame, true);

    RegisterGameEventListener("round_start");
    AddGameEventFireListener([](std::string event_name, IGameEvent* event, bool& dont_broadcast) -> int {
        if (event_name == "round_start") {
            static auto vgui = g_ifaceService.FetchInterface<IVGUI>(VGUI_INTERFACE_VERSION);

            vgui->ResetStateOfScreenTexts();

            timeoutsArray.push_back({ GetTime() + 100, []() -> void {
                vgui->RegenerateScreenTexts();
            } });

            processingTimeouts = true;
        }
        return 0;
    });
}

void CEventManager::Shutdown()
{
    auto networkserverservice = g_ifaceService.FetchInterface<INetworkServerService>(NETWORKSERVERSERVICE_INTERFACE_VERSION);
    auto server = g_ifaceService.FetchInterface<IServerGameDLL>(INTERFACEVERSION_SERVERGAMEDLL);

    SH_REMOVE_HOOK_MEMFUNC(IGameEventManager2, FireEvent, g_gameEventManager, this, &CEventManager::OnFireEvent, false);
    SH_REMOVE_HOOK_MEMFUNC(IGameEventManager2, FireEvent, g_gameEventManager, this, &CEventManager::OnFireEventPost, true);
    SH_REMOVE_HOOK_MEMFUNC(INetworkServerService, StartupServer, networkserverservice, this, &CEventManager::OnStartupServer, true);
    SH_REMOVE_HOOK_MEMFUNC(IServerGameDLL, GameFrame, server, this, &CEventManager::GameFrame, true);
    SH_REMOVE_HOOK_ID(g_uLoadEventFromFileHookID);
}

void CEventManager::GameFrame(bool simulate, bool first, bool last)
{
    if (processingTimeouts)
    {
        int64_t t = GetTime();
        for (auto it = timeoutsArray.begin(); it != timeoutsArray.end(); ++it) {
            if (it->first <= t) {
                queueRemoveTimeouts.push_back(it);
                it->second();
            }
        }

        for (auto it = queueRemoveTimeouts.rbegin(); it != queueRemoveTimeouts.rend(); ++it)
            timeoutsArray.erase(*it);

        queueRemoveTimeouts.clear();
        processingTimeouts = (timeoutsArray.size() > 0);
    }
}

int CEventManager::LoadEventsFromFile(const char* filePath, bool searchAll)
{
    if (!g_gameEventManager) {
        g_gameEventManager = META_IFACEPTR(IGameEventManager2);

        auto evmanager = g_ifaceService.FetchInterface<IEventManager>(GAMEEVENTMANAGER_INTERFACE_VERSION);
        evmanager->RegisterGameEventsListeners(false);

        SH_ADD_HOOK_MEMFUNC(IGameEventManager2, FireEvent, g_gameEventManager, this, &CEventManager::OnFireEvent, false);
        SH_ADD_HOOK_MEMFUNC(IGameEventManager2, FireEvent, g_gameEventManager, this, &CEventManager::OnFireEventPost, true);
    }

    // We don't need you anymore, stay here as it's free and you don't need to pay rent
/*
    if (!g_sDumpedFiles.contains(filePath))
    {
        g_sDumpedFiles.insert(filePath);

        auto logger = g_ifaceService.FetchInterface<ILogger>(LOGGER_INTERFACE_VERSION);
        logger->Info("Game Events", fmt::format("Dumping game events from file: '{}'...\n", filePath));

        auto filesystem = g_ifaceService.FetchInterface<IFileSystem>(FILESYSTEM_INTERFACE_VERSION);

        filesystem->CopyAFile(filePath, "GAME", GeneratePath(fmt::format("addons/swiftly/gamedata/cs2/gameevents{}", replace(filePath, Files::getBase(filePath), ""))).c_str(), false);

        logger->Info("Game Events", fmt::format("Dumped game events from file '{}'.\n", filePath));
    }
*/

    RETURN_META_VALUE(MRES_IGNORED, 0);
}

bool CEventManager::OnFireEvent(IGameEvent* pEvent, bool bDontBroadcast)
{
    if (!pEvent)
    {
        RETURN_META_VALUE(MRES_IGNORED, false);
    }

    std::string event_name = pEvent->GetName();
    bool shouldBroadcast = bDontBroadcast;
    for (const auto& [id, callback] : g_mEventListeners) {
        auto res = callback(event_name, pEvent, shouldBroadcast);
        if (res == 1) {
            g_sEventStack.push(g_gameEventManager->DuplicateEvent(pEvent));
            g_gameEventManager->FreeEvent(pEvent);
            RETURN_META_VALUE(MRES_SUPERCEDE, false);
        }
        else if (res == 2) break;
    }

    g_sEventStack.push(g_gameEventManager->DuplicateEvent(pEvent));

    if (shouldBroadcast != bDontBroadcast)
    {
        RETURN_META_VALUE_NEWPARAMS(MRES_IGNORED, true, &IGameEventManager2::FireEvent, (pEvent, shouldBroadcast));
    }

    RETURN_META_VALUE(MRES_IGNORED, true);
}

bool CEventManager::OnFireEventPost(IGameEvent* pEvent, bool bDontBroadcast)
{
    if (!pEvent)
    {
        RETURN_META_VALUE(MRES_IGNORED, false);
    }

    IGameEvent* realGameEvent = g_sEventStack.top();
    if (!realGameEvent) RETURN_META_VALUE(MRES_IGNORED, true);

    std::string event_name = realGameEvent->GetName();
    bool shouldBroadcast = bDontBroadcast;

    for (const auto& [id, callback] : g_mPostEventListeners) {
        auto res = callback(event_name, realGameEvent, shouldBroadcast);
        if (res == 1) {
            g_gameEventManager->FreeEvent(realGameEvent);
            g_sEventStack.pop();
            RETURN_META_VALUE(MRES_SUPERCEDE, false);
        }
        else if (res == 2) break;
    }

    g_gameEventManager->FreeEvent(realGameEvent);
    g_sEventStack.pop();

    RETURN_META_VALUE(MRES_IGNORED, true);
}

void CEventManager::OnStartupServer(const GameSessionConfiguration_t& config, ISource2WorldSession*, const char*)
{
    auto evmanager = g_ifaceService.FetchInterface<IEventManager>(GAMEEVENTMANAGER_INTERFACE_VERSION);
    evmanager->RegisterGameEventsListeners(true);
}

void CEventManager::RegisterGameEventsListeners(bool shouldRegister)
{
    QueueLockGuard lock(m_mtxLock);
    if (!g_gameEventManager) return;

    if (shouldRegister && !g_bEventsLoaded) {
        g_bEventsLoaded = true;

        for (auto it = g_sEnqueueListenEvents.begin(); it != g_sEnqueueListenEvents.end(); ++it)
            RegisterGameEventListener(*it);

        g_sEnqueueListenEvents.clear();
    }
}

void CEventManager::RegisterGameEventListener(std::string event_name)
{
    QueueLockGuard lock(m_mtxLock);
    if (!g_bEventsLoaded) {
        g_sEnqueueListenEvents.insert(event_name);
    }
    else {
        if (!g_gameEventManager) return;

        if (!g_gameEventManager->FindListener(this, event_name.c_str()))
            g_gameEventManager->AddListener(this, event_name.c_str(), true);

        auto logger = g_ifaceService.FetchInterface<ILogger>(LOGGER_INTERFACE_VERSION);
        logger->Debug("Game Events", fmt::format("Registered listener for event '{}'.\n", event_name));
    }
}

uint64_t CEventManager::AddGameEventFireListener(std::function<int(std::string, IGameEvent*, bool&)> callback)
{
    QueueLockGuard lock(m_mtxLock);
    static uint64_t s_uiListenerID = 0;
    g_mEventListeners[++s_uiListenerID] = callback;
    return s_uiListenerID;
}

uint64_t CEventManager::AddPostGameEventFireListener(std::function<int(std::string, IGameEvent*, bool&)> callback)
{
    QueueLockGuard lock(m_mtxLock);
    static uint64_t s_uiListenerID = 0;
    g_mPostEventListeners[++s_uiListenerID] = callback;
    return s_uiListenerID;
}

void CEventManager::RemoveGameEventFireListener(uint64_t listener_id)
{
    QueueLockGuard lock(m_mtxLock);
    auto it = g_mEventListeners.find(listener_id);
    if (it != g_mEventListeners.end())
    {
        g_mEventListeners.erase(it);
    }
}

void CEventManager::RemovePostGameEventFireListener(uint64_t listener_id)
{
    QueueLockGuard lock(m_mtxLock);
    auto it = g_mPostEventListeners.find(listener_id);
    if (it != g_mPostEventListeners.end())
    {
        g_mPostEventListeners.erase(it);
    }
}

IGameEventManager2* CEventManager::GetGameEventManager()
{
    return g_gameEventManager;
}

void CEventManager::FireGameEvent(IGameEvent* event) {}