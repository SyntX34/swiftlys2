/************************************************************************************************
 * SwiftlyS2 is a scripting framework for Source2-based games.
 * Copyright (C) 2023-2026 Swiftly Solution SRL via Sava Andrei-Sebastian and it's contributors
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

#include <api/interfaces/manager.h>
#include <scripting/scripting.h>

#include <api/shared/string.h>

int Bridge_Commands_HandleCommandForPlayer(int playerid, const char* command)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    if (!servercommands)
    {
        return -1;
    }

    return servercommands->HandleCommand(playerid, command, false);
}

uint64_t Bridge_Commands_RegisterCommand(const char* commandName, bool registerRaw, const char* helpText)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    if (!servercommands)
    {
        return 0;
    }

    return servercommands->RegisterCommand(commandName, registerRaw, helpText);
}

void Bridge_Commands_SetCommandHandler(void* callback)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    if (!servercommands)
    {
        return;
    }

    servercommands->SetCommandHandler(
        [callback](std::string commandName, int playerid, std::vector<std::string> args, std::string originalCommandName, std::string selectedPrefix, bool isSilentCommand) -> void
        {
            static std::string cmd_name;
            cmd_name = commandName;

            static std::string imploded_args;
            imploded_args = implode(args, "\x01");

            static std::string original_name;
            original_name = originalCommandName;

            static std::string selected_prefix;
            selected_prefix = selectedPrefix;

            reinterpret_cast<void (*)(const char*, int, const char*, const char*, const char*, uint8_t)>(callback)(cmd_name.c_str(), playerid, imploded_args.c_str(), original_name.c_str(), selected_prefix.c_str(), isSilentCommand == true ? 1 : 0);
        });
}

void Bridge_Commands_UnregisterCommand(uint64_t callbackID)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    servercommands->UnregisterCommand(callbackID);
}

uint8_t Bridge_Commands_IsCommandRegistered(const char* commandName)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    return servercommands->IsCommandRegistered(commandName) ? 1 : 0;
}

uint64_t Bridge_Commands_RegisterAlias(const char* alias, const char* command, bool registerRaw)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    return servercommands->RegisterAlias(alias, command, registerRaw);
}

void Bridge_Commands_UnregisterAlias(uint64_t callbackID)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    servercommands->UnregisterAlias(callbackID);
}

void Bridge_Commands_SetClientCommandHandler(void* callback)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    if (!servercommands)
        return;

    servercommands->SetClientCommandHandler([callback](int playerid, const std::string& command) -> int {
        return reinterpret_cast<int (*)(int, const char*)>(callback)(playerid, command.c_str());
        });
}

uint64_t Bridge_Commands_RegisterClientChatListener(void* callback)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    return servercommands->RegisterClientChatListener([callback](int playerid, const std::string& text, bool teamonly) -> int { return reinterpret_cast<int (*)(int, const char*, uint8_t)>(callback)(playerid, text.c_str(), teamonly ? 1 : 0); });
}

void Bridge_Commands_UnregisterClientChatListener(uint64_t callbackID)
{
    auto servercommands = g_ifaceService.FetchInterface<IServerCommands>(SERVERCOMMANDS_INTERFACE_VERSION);
    servercommands->UnregisterClientChatListener(callbackID);
}

DEFINE_NATIVE("Commands.HandleCommandForPlayer", Bridge_Commands_HandleCommandForPlayer);
DEFINE_NATIVE("Commands.RegisterCommand", Bridge_Commands_RegisterCommand);
DEFINE_NATIVE("Commands.SetCommandHandler", Bridge_Commands_SetCommandHandler);
DEFINE_NATIVE("Commands.UnregisterCommand", Bridge_Commands_UnregisterCommand);
DEFINE_NATIVE("Commands.RegisterAlias", Bridge_Commands_RegisterAlias);
DEFINE_NATIVE("Commands.UnregisterAlias", Bridge_Commands_UnregisterAlias);
DEFINE_NATIVE("Commands.SetClientCommandHandler", Bridge_Commands_SetClientCommandHandler);
DEFINE_NATIVE("Commands.RegisterClientChatListener", Bridge_Commands_RegisterClientChatListener);
DEFINE_NATIVE("Commands.UnregisterClientChatListener", Bridge_Commands_UnregisterClientChatListener);
DEFINE_NATIVE("Commands.IsCommandRegistered", Bridge_Commands_IsCommandRegistered);
