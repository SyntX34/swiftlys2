using System.Runtime.InteropServices;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Core.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.Profiler;
using SwiftlyS2.Shared.Commands;
using SwiftlyS2.Shared.Permissions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwiftlyS2.Core.Models;
using SwiftlyS2.Core.Translations;

namespace SwiftlyS2.Core.Commands;

internal delegate void GlobalCommandHandlerDelegate( nint commandName, int playerId, nint args, nint originalCommandName, nint prefix, byte silent );
internal delegate HookResult ClientCommandListenerCallbackDelegate( int playerId, nint commandLine );
internal delegate HookResult ClientChatListenerCallbackDelegate( int playerId, nint text, byte teamonly );

internal abstract class CommandCallbackBase : IDisposable
{
    public Guid Guid { get; protected init; }
    public string PluginName { get; protected init; }
    public IContextedProfilerService Profiler { get; }
    public ILoggerFactory LoggerFactory { get; }

    protected CommandCallbackBase( ILoggerFactory loggerFactory, IContextedProfilerService profiler, string pluginName )
    {
        LoggerFactory = loggerFactory;
        Profiler = profiler;
        PluginName = pluginName;
    }

    public abstract void Dispose();
}

internal class CommandCallback : CommandCallbackBase
{
    public string CommandName { get; protected init; }
    public bool RegisterRaw { get; protected init; }
    public string Permission { get; protected init; }
    public string HelpText { get; protected init; }

    private readonly ICommandService.CommandListener commandHandle;
    private readonly ulong nativeListenerId;
    private readonly ILogger<CommandCallback> logger;
    private readonly IPlayerManagerService playerManagerService;
    private readonly IPermissionManager permissionManager;
    private readonly IOptionsMonitor<CommandOverrideConfig> commandOverrideOptions;

    public CommandCallback( string commandName, bool registerRaw, ICommandService.CommandListener handler, string permission, string helpText, IPlayerManagerService playerManagerService, IPermissionManager permissionManager, IOptionsMonitor<CommandOverrideConfig> commandOverrideOptions, ILoggerFactory loggerFactory, IContextedProfilerService profiler, string pluginName ) : base(loggerFactory, profiler, pluginName)
    {
        this.logger = LoggerFactory.CreateLogger<CommandCallback>();
        this.playerManagerService = playerManagerService;
        this.permissionManager = permissionManager;
        this.commandOverrideOptions = commandOverrideOptions;

        Guid = Guid.NewGuid();

        CommandName = commandName;
        RegisterRaw = registerRaw;
        Permission = permission;
        HelpText = helpText;
        commandHandle = handler;

        nativeListenerId = NativeCommands.RegisterCommand(commandName, registerRaw, helpText);
    }

    internal void Invoke( int playerId, string[] args, string originalCommandName, string prefix, bool silent )
    {
        var category = "CommandCallback::" + CommandName;
        Profiler.StartRecording(category);

        try
        {
            var context = new CommandContext(playerId, args, originalCommandName, prefix, silent);
            var hasOverride = commandOverrideOptions.CurrentValue.Permissions.TryGetValue(originalCommandName, out var overriddenPermission);
            var requiredPermission = hasOverride ? overriddenPermission : Permission;
            if (!context.IsSentByPlayer || string.IsNullOrWhiteSpace(requiredPermission) || permissionManager.PlayerHasPermission(playerManagerService.GetPlayer(playerId)?.SteamID ?? 0, requiredPermission))
            {
                commandHandle(context);
            }
            else
            {
                context.Reply(GlobalLocalization.PermissionCommandDenied());
            }
        }
        catch (Exception e)
        {
            if (!GlobalExceptionHandler.Handle(ref e)) return;
            logger.LogError(e, "Failed to handle command {CommandName}.", CommandName);
        }
        finally
        {
            Profiler.StopRecording(category);
        }
    }

    public override void Dispose()
    {
        NativeCommands.UnregisterCommand(nativeListenerId);
    }
}

internal class ClientCommandListenerCallback : CommandCallbackBase
{
    private readonly ICommandService.ClientCommandHandler commandHandle;
    private readonly ILogger<ClientCommandListenerCallback> logger;

    public ClientCommandListenerCallback( ICommandService.ClientCommandHandler handler, ILoggerFactory loggerFactory, IContextedProfilerService profiler, string pluginName ) : base(loggerFactory, profiler, pluginName)
    {
        logger = LoggerFactory.CreateLogger<ClientCommandListenerCallback>();
        Guid = Guid.NewGuid();
        commandHandle = handler;
    }

    internal HookResult Invoke( int playerId, string commandLine )
    {
        var category = "ClientCommandListenerCallback";
        try
        {
            Profiler.StartRecording(category);
            var result = commandHandle(playerId, commandLine);
            Profiler.StopRecording(category);
            return result;
        }
        catch (Exception e)
        {
            if (!GlobalExceptionHandler.Handle(ref e)) return HookResult.Continue;
            logger.LogError(e, "Failed to handle client command listener.");
            return HookResult.Continue;
        }
        finally
        {
            Profiler.StopRecording(category);
        }
    }

    public override void Dispose() { }
}

internal class ClientChatListenerCallback : CommandCallbackBase
{
    private readonly ICommandService.ClientChatHandler commandHandle;
    private readonly ClientChatListenerCallbackDelegate commandCallback;
    private readonly nint commandCallbackPtr;
    private readonly ulong nativeListenerId;
    private readonly ILogger<ClientChatListenerCallback> logger;

    public ClientChatListenerCallback( ICommandService.ClientChatHandler handler, ILoggerFactory loggerFactory, IContextedProfilerService profiler, string pluginName ) : base(loggerFactory, profiler, pluginName)
    {
        logger = LoggerFactory.CreateLogger<ClientChatListenerCallback>();
        Guid = Guid.NewGuid();

        commandHandle = handler;
        commandCallback = ( playerId, textPtr, teamonly ) =>
        {
            try
            {
                var category = "ClientChatListenerCallback";
                Profiler.StartRecording(category);
                var textString = Marshal.PtrToStringUTF8(textPtr)!;
                var result = commandHandle(playerId, textString, teamonly == 1);
                Profiler.StopRecording(category);
                return result;
            }
            catch (Exception e)
            {
                if (!GlobalExceptionHandler.Handle(ref e)) return HookResult.Continue;
                logger.LogError(e, "Failed to handle client chat listener.");
                return HookResult.Continue;
            }
        };

        commandCallbackPtr = Marshal.GetFunctionPointerForDelegate(commandCallback);
        nativeListenerId = NativeCommands.RegisterClientChatListener(commandCallbackPtr);
    }

    public override void Dispose()
    {
        NativeCommands.UnregisterClientChatListener(nativeListenerId);
    }
}