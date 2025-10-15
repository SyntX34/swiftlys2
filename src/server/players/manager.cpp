/************************************************************************************************
 * SwiftlyS2 is a scripting framework for Source2-based games.
 * Copyright (C) 2025 Swiftly Solution SRL via Sava Andrei-Sebastian and it's contributors
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 ************************************************************************************************/

#include "manager.h"

#include <api/interfaces/manager.h>

#include <core/bridge/metamod.h>
#include <core/entrypoint.h>

#include "cs_usercmd.pb.h"
#include "usercmd.pb.h"

class EntityCheckTransmit
{
public:
    CBitVec<MAX_EDICTS>* m_pTransmitEntity;	// 0
    CBitVec<MAX_EDICTS>* m_pUnkBitVec;		// 8
    CBitVec<MAX_EDICTS>* m_pUnkBitVec2;		// 16
    CBitVec<MAX_EDICTS>* m_pUnkBitVec3;		// 24
    CBitVec<MAX_EDICTS>* m_pTransmitAlways; // 32
    CUtlVector<int> m_unk40;				// 40
    vis_info_t* m_VisInfo;					// 64
    [[maybe_unused]] byte m_unk72[0x1F8];	// 72
    CEntityIndex m_nClientEntityIndex;		// 576
    bool m_bFullUpdate;						// 580
};

class CUserCmd
{
public:
    [[maybe_unused]] char pad0[0x10];
    CSGOUserCmdPB cmd;
    [[maybe_unused]] char pad1[0x38];
#ifdef _WIN32
    [[maybe_unused]] char pad2[0x8];
#endif
};

IFunctionHook* g_pProcessUserCmdsHook = nullptr;
IVFunctionHook* g_pOnGameFramePlayerHook = nullptr;

IVFunctionHook* g_pClientConnectHook = nullptr;
IVFunctionHook* g_pOnClientConnectedHook = nullptr;
IVFunctionHook* g_pClientDisconnectHook = nullptr;
IVFunctionHook* g_pClientPutInServerHook = nullptr;

IVFunctionHook* g_pCheckTransmitHook = nullptr;

void* ProcessUsercmdsHook(void* pController, CUserCmd* cmds, int numcmds, bool paused, float margin);
void OnGameFramePlayerHook(void* _this, bool simulate, bool first, bool last);

void OnClientPutInServerHook(void* _this, CPlayerSlot slot, char const* pszName, int type, uint64 xuid);
bool ClientConnectHook(void* _this, CPlayerSlot slot, const char* pszName, uint64 xuid, const char* pszNetworkID, bool unk1, CBufferString* pRejectReason);
void OnClientConnectedHook(void* _this, CPlayerSlot slot, const char* pszName, uint64 xuid, const char* pszNetworkID, const char* pszAddress, bool bFakePlayer);
void ClientDisconnectHook(void* _this, CPlayerSlot slot, ENetworkDisconnectionReason reason, const char* pszName, uint64 xuid, const char* pszNetworkID);
void CheckTransmitHook(CCheckTransmitInfo** ppInfoList, int infoCount, CBitVec<16384>& unionTransmitEdicts, CBitVec<16384>& unk, const Entity2Networkable_t** pNetworkables, const uint16_t* pEntityIndicies, int nEntities);

void CPlayerManager::Initialize()
{
    g_Players = new CPlayer * [g_SwiftlyCore.GetMaxGameClients()];
    for (int i = 0; i < g_SwiftlyCore.GetMaxGameClients(); i++) {
        g_Players[i] = nullptr;
    }

    auto gamedata = g_ifaceService.FetchInterface<IGameDataManager>(GAMEDATA_INTERFACE_VERSION);
    auto hooksmanager = g_ifaceService.FetchInterface<IHooksManager>(HOOKSMANAGER_INTERFACE_VERSION);

    auto gameentities = g_ifaceService.FetchInterface<ISource2GameEntities>(SOURCE2GAMEENTITIES_INTERFACE_VERSION);

    g_pClientConnectHook = hooksmanager->CreateVFunctionHook();
    g_pClientConnectHook->SetHookFunction(INTERFACEVERSION_SERVERGAMECLIENTS, gamedata->GetOffsets()->Fetch("IServerGameClients::ClientConnect"), reinterpret_cast<void*>(ClientConnectHook));
    g_pClientConnectHook->Enable();

    g_pOnClientConnectedHook = hooksmanager->CreateVFunctionHook();
    g_pOnClientConnectedHook->SetHookFunction(INTERFACEVERSION_SERVERGAMECLIENTS, gamedata->GetOffsets()->Fetch("IServerGameClients::OnClientConnected"), reinterpret_cast<void*>(OnClientConnectedHook));
    g_pOnClientConnectedHook->Enable();

    g_pClientDisconnectHook = hooksmanager->CreateVFunctionHook();
    g_pClientDisconnectHook->SetHookFunction(INTERFACEVERSION_SERVERGAMECLIENTS, gamedata->GetOffsets()->Fetch("IServerGameClients::ClientDisconnect"), reinterpret_cast<void*>(ClientDisconnectHook));
    g_pClientDisconnectHook->Enable();

    g_pClientPutInServerHook = hooksmanager->CreateVFunctionHook();
    g_pClientPutInServerHook->SetHookFunction(INTERFACEVERSION_SERVERGAMECLIENTS, gamedata->GetOffsets()->Fetch("IServerGameClients::ClientPutInServer"), reinterpret_cast<void*>(OnClientPutInServerHook));
    g_pClientPutInServerHook->Enable();

    g_pCheckTransmitHook = hooksmanager->CreateVFunctionHook();
    g_pCheckTransmitHook->SetHookFunction(SOURCE2GAMEENTITIES_INTERFACE_VERSION, gamedata->GetOffsets()->Fetch("ISource2GameEntities::CheckTransmit"), reinterpret_cast<void*>(CheckTransmitHook));
    g_pCheckTransmitHook->Enable();

    auto processusercmds = gamedata->GetSignatures()->Fetch("CCSPlayerController::ProcessUserCmd");

    g_pOnGameFramePlayerHook = hooksmanager->CreateVFunctionHook();
    g_pOnGameFramePlayerHook->SetHookFunction(INTERFACEVERSION_SERVERGAMEDLL, gamedata->GetOffsets()->Fetch("IServerGameDLL::GameFrame"), reinterpret_cast<void*>(OnGameFramePlayerHook));
    g_pOnGameFramePlayerHook->Enable();

    g_pProcessUserCmdsHook = hooksmanager->CreateFunctionHook();
    g_pProcessUserCmdsHook->SetHookFunction(processusercmds, reinterpret_cast<void*>(ProcessUsercmdsHook));
    g_pProcessUserCmdsHook->Enable();
}

void CPlayerManager::Shutdown() {
    for (int i = 0; i < g_SwiftlyCore.GetMaxGameClients(); i++) {
        if (g_Players[i] != nullptr) {
            delete g_Players[i];
        }
    }
    delete[] g_Players;

    auto hooksmanager = g_ifaceService.FetchInterface<IHooksManager>(HOOKSMANAGER_INTERFACE_VERSION);

    if (g_pOnGameFramePlayerHook)
    {
        g_pOnGameFramePlayerHook->Disable();
        hooksmanager->DestroyVFunctionHook(g_pOnGameFramePlayerHook);
        g_pOnGameFramePlayerHook = nullptr;
    }

    if (g_pProcessUserCmdsHook)
    {
        g_pProcessUserCmdsHook->Disable();
        hooksmanager->DestroyFunctionHook(g_pProcessUserCmdsHook);
        g_pProcessUserCmdsHook = nullptr;
    }

    if (g_pClientConnectHook)
    {
        g_pClientConnectHook->Disable();
        hooksmanager->DestroyVFunctionHook(g_pClientConnectHook);
        g_pClientConnectHook = nullptr;
    }

    if (g_pOnClientConnectedHook)
    {
        g_pOnClientConnectedHook->Disable();
        hooksmanager->DestroyVFunctionHook(g_pOnClientConnectedHook);
        g_pOnClientConnectedHook = nullptr;
    }

    if (g_pClientDisconnectHook)
    {
        g_pClientDisconnectHook->Disable();
        hooksmanager->DestroyVFunctionHook(g_pClientDisconnectHook);
        g_pClientDisconnectHook = nullptr;
    }

    if (g_pClientPutInServerHook)
    {
        g_pClientPutInServerHook->Disable();
        hooksmanager->DestroyVFunctionHook(g_pClientPutInServerHook);
        g_pClientPutInServerHook = nullptr;
    }

    if (g_pCheckTransmitHook)
    {
        g_pCheckTransmitHook->Disable();
        hooksmanager->DestroyVFunctionHook(g_pCheckTransmitHook);
        g_pCheckTransmitHook = nullptr;
    }
}

extern void* g_pOnClientPutInServerCallback;

void OnClientPutInServerHook(void* _this, CPlayerSlot slot, char const* pszName, int type, uint64 xuid)
{
    reinterpret_cast<decltype(&OnClientPutInServerHook)>(g_pClientPutInServerHook->GetOriginal())(_this, slot, pszName, type, xuid);

    if (g_pOnClientPutInServerCallback)
        reinterpret_cast<void(*)(int, int)>(g_pOnClientPutInServerCallback)(slot.Get(), type);
}

extern void* g_pOnClientProcessUsercmdsCallback;

void* ProcessUsercmdsHook(void* pController, CUserCmd* cmds, int numcmds, bool paused, float margin)
{
    auto playerid = ((CEntityInstance*)pController)->m_pEntity->m_EHandle.GetEntryIndex() - 1;

    google::protobuf::Message** pMsg = new google::protobuf::Message * [numcmds];
    for (int i = 0; i < numcmds; i++)
        pMsg[i] = (google::protobuf::Message*)&cmds[i].cmd;

    if (g_pOnClientProcessUsercmdsCallback)
        reinterpret_cast<void(*)(int, void*, int, bool, float)>(g_pOnClientProcessUsercmdsCallback)(playerid, pMsg, numcmds, paused, margin);

    delete[] pMsg;

    return reinterpret_cast<void* (*)(void*, CUserCmd*, int, bool, float)>(g_pProcessUserCmdsHook->GetOriginal())(pController, cmds, numcmds, paused, margin);
}

void CheckTransmitHook(CCheckTransmitInfo** ppInfoList, int infoCount, CBitVec<16384>& unionTransmitEdicts, CBitVec<16384>& unk, const Entity2Networkable_t** pNetworkables, const uint16_t* pEntityIndicies, int nEntities)
{
    reinterpret_cast<decltype(&CheckTransmitHook)>(g_pCheckTransmitHook->GetOriginal())(ppInfoList, infoCount, unionTransmitEdicts, unk, pNetworkables, pEntityIndicies, nEntities);

    static auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    for (int i = 0; i < infoCount; i++)
    {
        auto& pInfo = (EntityCheckTransmit*&)ppInfoList[i];
        int playerid = pInfo->m_nClientEntityIndex.Get();
        if (!playermanager->IsPlayerOnline(playerid)) continue;
        auto player = playermanager->GetPlayer(playerid);

        auto& blockedBits = player->GetBlockedTransmittingBits();

        uint64_t* base = reinterpret_cast<uint64_t*>(pInfo->m_pTransmitEntity->Base());
        uint64_t* baseAlways = reinterpret_cast<uint64_t*>(pInfo->m_pTransmitAlways->Base());
        auto& activeMasks = blockedBits.activeMasks;

        // NUM_MASKS_ACTIVE ops = NUM_MASKS_ACTIVE*64 bits -> 64 players -> NUM_MASKS_ACTIVE*64 ops
        for (auto& dword : activeMasks) {
            base[dword] &= ~blockedBits.blockedMask[dword];
            baseAlways[dword] &= ~blockedBits.blockedMask[dword];
        }

        // 512 ops = 16k bits -> 64 players -> 32k ops
        // for (int i = pInfo->m_pTransmitEntity->GetNumDWords() - 1; i >= 0; i--) {
        //     uint32_t& word = base[i];
        //     uint32_t& wordAlways = baseAlways[i];

        //     word &= ~blockedBase[i];
        //     wordAlways &= ~blockedBase[i];
        // }

        //16k ops = 16k bits -> 64 players -> 1M ops
        /*
        for (int i = 0; i < 16384; i++)
            if (blockedBits.IsBitSet(i))
                pInfo->m_pTransmitEntity->Clear(i);
        */
    }
}

extern void* g_pOnGameTickCallback;

void OnGameFramePlayerHook(void* _this, bool simulate, bool first, bool last)
{
    reinterpret_cast<decltype(&OnGameFramePlayerHook)>(g_pOnGameFramePlayerHook->GetOriginal())(_this, simulate, first, last);

    static auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    static auto vgui = g_ifaceService.FetchInterface<IVGUI>(VGUI_INTERFACE_VERSION);

    if (g_pOnGameTickCallback) reinterpret_cast<void(*)(bool, bool, bool)>(g_pOnGameTickCallback)(simulate, first, last);

    for (int i = 0; i < 64; i++)
        if (playermanager->IsPlayerOnline(i)) {
            auto player = playermanager->GetPlayer(i);
            if (!player) continue;
            player->Think();
        }

    vgui->Update();
}

extern void* g_pOnClientConnectCallback;

bool ClientConnectHook(void* _this, CPlayerSlot slot, const char* pszName, uint64 xuid, const char* pszNetworkID, bool unk1, CBufferString* pRejectReason)
{
    static auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    auto playerid = slot.Get();
    auto player = playermanager->RegisterPlayer(playerid);
    player->Initialize(playerid);
    player->SetUnauthorizedSteamID(xuid);

    if (g_pOnClientConnectCallback)
        if (reinterpret_cast<bool(*)(int)>(g_pOnClientConnectCallback)(playerid) == false)
            return false;

    return reinterpret_cast<decltype(&ClientConnectHook)>(g_pClientConnectHook->GetOriginal())(_this, slot, pszName, xuid, pszNetworkID, unk1, pRejectReason);
}

void OnClientConnectedHook(void* _this, CPlayerSlot slot, const char* pszName, uint64 xuid, const char* pszNetworkID, const char* pszAddress, bool bFakePlayer)
{
    static auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    auto playerid = slot.Get();
    if (bFakePlayer) {
        auto player = playermanager->RegisterPlayer(playerid);
        player->Initialize(playerid);
    }
    else {
        auto cvarmanager = g_ifaceService.FetchInterface<IConvarManager>(CONVARMANAGER_INTERFACE_VERSION);
        cvarmanager->QueryClientConvar(playerid, "cl_language");
    }

    reinterpret_cast<decltype(&OnClientConnectedHook)>(g_pOnClientConnectedHook->GetOriginal())(_this, slot, pszName, xuid, pszNetworkID, pszAddress, bFakePlayer);
}

extern void* g_pOnClientDisconnectCallback;

void ClientDisconnectHook(void* _this, CPlayerSlot slot, ENetworkDisconnectionReason reason, const char* pszName, uint64 xuid, const char* pszNetworkID)
{
    reinterpret_cast<decltype(&ClientDisconnectHook)>(g_pClientDisconnectHook->GetOriginal())(_this, slot, reason, pszName, xuid, pszNetworkID);

    static auto playermanager = g_ifaceService.FetchInterface<IPlayerManager>(PLAYERMANAGER_INTERFACE_VERSION);
    auto playerid = slot.Get();

    if (g_pOnClientDisconnectCallback)
        reinterpret_cast<void(*)(int, int)>(g_pOnClientDisconnectCallback)(playerid, reason);

    playermanager->UnregisterPlayer(playerid);
}

IPlayer* CPlayerManager::RegisterPlayer(int playerid)
{
    if (playerid < 0 || playerid >= g_SwiftlyCore.GetMaxGameClients()) return nullptr;

    if (g_Players[playerid] != nullptr) UnregisterPlayer(playerid);

    g_Players[playerid] = new CPlayer();
    g_Players[playerid]->Initialize(playerid);

    return g_Players[playerid];
}

void CPlayerManager::UnregisterPlayer(int playerid)
{
    if (playerid < 0 || playerid >= g_SwiftlyCore.GetMaxGameClients()) return;
    if (g_Players[playerid] == nullptr) return;

    static auto vgui = g_ifaceService.FetchInterface<IVGUI>(VGUI_INTERFACE_VERSION);

    vgui->UnregisterForPlayer(g_Players[playerid]);

    g_Players[playerid]->Shutdown();
    delete g_Players[playerid];
    g_Players[playerid] = nullptr;
}

IPlayer* CPlayerManager::GetPlayer(int playerid)
{
    if (playerid < 0 || playerid >= g_SwiftlyCore.GetMaxGameClients()) return nullptr;
    if (IsPlayerOnline(playerid)) return g_Players[playerid];
    return nullptr;
}

bool CPlayerManager::IsPlayerOnline(int playerid)
{
    if (playerid < 0 || playerid >= g_SwiftlyCore.GetMaxGameClients()) return false;
    static auto engine = g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER);
    return (engine->GetClientSteamID(playerid) != nullptr);
}

int CPlayerManager::GetPlayerCount()
{
    auto engine = g_ifaceService.FetchInterface<IVEngineServer2>(INTERFACEVERSION_VENGINESERVER);
    int count = 0;

    for (int i = 0; i < GetPlayerCap(); i++)
        if (engine->GetClientSteamID(i)) ++count;

    return count;
}

int CPlayerManager::GetPlayerCap()
{
    return g_SwiftlyCore.GetMaxGameClients();
}

void CPlayerManager::SendMsg(MessageType type, const std::string& message)
{
    for (int i = 0; i < g_SwiftlyCore.GetMaxGameClients(); i++) {
        IPlayer* player = GetPlayer(i);
        if (player) player->SendMsg(type, message);
    }
}

void CPlayerManager::SteamAPIServerActivated()
{
    m_CallbackValidateAuthTicketResponse.Register(this, &CPlayerManager::OnValidateAuthTicket);
}

void CPlayerManager::OnValidateAuthTicket(ValidateAuthTicketResponse_t* response)
{
    uint64_t steamid = response->m_SteamID.ConvertToUint64();

    for (int i = 0; i < GetPlayerCap(); i++) {
        auto player = GetPlayer(i);
        if (!player) continue;
        if (player->GetUnauthorizedSteamID() != steamid) continue;

        player->ChangeAuthorizationState(response->m_eAuthSessionResponse == k_EAuthSessionResponseOK);
        break;
    }
}