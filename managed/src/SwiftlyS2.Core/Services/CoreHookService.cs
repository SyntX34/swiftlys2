
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using SwiftlyS2.Core.Datamaps;
using SwiftlyS2.Core.EntitySystem;
using SwiftlyS2.Core.Events;
using SwiftlyS2.Core.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Memory;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.ProtobufDefinitions;
using SwiftlyS2.Shared.SchemaDefinitions;
using SwiftlyS2.Shared.SteamAPI;

namespace SwiftlyS2.Core.Services;

internal class CoreHookService : IDisposable
{
    private readonly ISwiftlyCore core;
    private readonly ILogger<CoreHookService> logger;
    private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public CoreHookService( ILogger<CoreHookService> logger, ISwiftlyCore core )
    {
        this.logger = logger;
        this.core = core;

        HookExecuteCommand();
        HookICvarFindConCommandTemplate();
        HookCCSPlayerItemServicesCanAcquire();
        HookCCSPlayerWeaponServicesCanUse();
        HookCBaseEntityTouchTemplate();
        HookSteamServerAPIActivated();
        HookCPlayerMovementServicesRunCommand();
        HookCCSPlayerPawnPostThink();
        HookCBaseEntityTakeDamage();
        HookEntityIdentityAcceptInput();
        HookEntityIOOutputFireOutputInternal();
        HookDispatchDatamapFunction();
        HookWeaponServicesDropWeapon();
        HookOnClientProcessUsercmds();
    }

    /*
        Original function in engine2.dll: __int64 sub_1C0CD0(__int64 a1, int a2, unsigned int a3, ...)
        This is a variadic function, but we only need the first two variable arguments (v55, v57)

        __int64 sub_1C0CD0(__int64 a1, int a2, unsigned int a3, ...)
        {
            ...

            va_list va; // [rsp+D28h] [rbp+D28h]
            __int64 v55; // [rsp+E28h] [rbp+D28h] BYREF
            va_list va1; // [rsp+E28h] [rbp+D28h]

            ...

            va_start(va1, a3);
            va_start(va, a3);
            v55 = va_arg(va1, _QWORD);
            v57 = va_arg(va1, _QWORD);

            ...
        }

        So we model it as a fixed 5-parameter function for interop purposes
    */
    private delegate nint ExecuteCommand( nint a1, int a2, uint a3, nint a4, nint a5 );
    private delegate nint ICvarFindConCommandWindows( nint pICvar, nint pRet, nint pConCommandName, int unk1 );
    private delegate nint ICvarFindConCommandLinux( nint pICvar, nint pConCommandName, int unk1 );
    private delegate void SteamServerAPIActivated( nint pServer );
    private delegate void DispatchDatamapFunction( nint a1, nint pDatamapFunc, nint a3, uint a4, nint a5, double a6 /* unknown */ );

    private IUnmanagedFunction<ExecuteCommand>? executeCommand;
    private Guid executeCommandGuid;
    private IUnmanagedFunction<ICvarFindConCommandWindows>? findConCommandWindows;
    private IUnmanagedFunction<ICvarFindConCommandLinux>? findConCommandLinux;
    private Guid findConCommandGuid;
    private IUnmanagedFunction<SteamServerAPIActivated>? steamServerAPIActivated;
    private Guid steamServerAPIActivatedGuid;
    private IUnmanagedFunction<DispatchDatamapFunction>? dispatchDatamapFunction;
    private Guid dispatchDatamapFunctionGuid;

    internal unsafe void EntityAcceptInputPre( ref AcceptInputEntityPreContext @event )
    {
        if (!EventPublisher.ListensToAcceptInput) return;

        var @e = new OnEntityIdentityAcceptInputHookEvent {
            Identity = @event.Params.Identity,
            EntityInstance = @event.Params.EntityInstance,
            DesignerName = @event.Params.DesignerName,
            InputName = @event.Params.InputName,
            Activator = @event.Params.Activator,
            Caller = @event.Params.Caller,
            _variant = @event.Params._variant,
            OutputId = @event.Params.OutputId,
            Result = HookResult.Continue
        };
        EventPublisher.InvokeOnEntityIdentityAcceptInputHook(@e);
        @event.SetHookResult(@e.Result);
    }

    internal void HookEntityIdentityAcceptInput()
    {
        core.GameHooks.Entities.AcceptInput.Pre += EntityAcceptInputPre;
    }

    internal void UnhookEntityIdentityAcceptInput()
    {
        core.GameHooks.Entities.AcceptInput.Pre -= EntityAcceptInputPre;
    }

    internal unsafe void EntityFireOutputPre( ref FireOutputEntityPreContext @event )
    {
        if (!EventPublisher.ListensToFireOutput) return;

        var @e = new OnEntityFireOutputHookEvent {
            _entityIO = @event.Params._entityIO,
            _variant = @event.Params._variant,
            DesignerName = @event.Params.DesignerName,
            OutputName = @event.Params.OutputName,
            Activator = @event.Params.Activator,
            Caller = @event.Params.Caller,
            Delay = @event.Params.Delay,
            Result = HookResult.Continue
        };
        EventPublisher.InvokeEntityFireOutputHook(@e);
        @event.SetHookResult(@e.Result);
    }

    internal void HookEntityIOOutputFireOutputInternal()
    {
        core.GameHooks.Entities.FireOutput.Pre += EntityFireOutputPre;
    }

    internal void UnhookEntityIOOutputFireOutputInternal()
    {
        core.GameHooks.Entities.FireOutput.Pre -= EntityFireOutputPre;
    }

    internal void HookExecuteCommand()
    {
        var address = core.GameData.GetSignature("Cmd_ExecuteCommand");

        logger.LogInformation("Hooking Cmd_ExecuteCommand at {Address:X}", address);

        executeCommand = core.Memory.GetUnmanagedFunctionByAddress<ExecuteCommand>(address);
        executeCommandGuid = executeCommand.AddHook(( next ) =>
        {
            return ( a1, a2, a3, a4, a5 ) =>
            {
                unsafe
                {
                    if (a5 != nint.Zero)
                    {
                        ref var command = ref Unsafe.AsRef<CCommand>((void*)a5);
                        var @eventPre = new OnCommandExecuteHookEvent(ref command, HookMode.Pre);
                        EventPublisher.InvokeOnCommandExecuteHook(@eventPre);

                        if (@eventPre.Result == HookResult.Stop || @eventPre.Result == HookResult.CancelOriginal)
                        {
                            return 0;
                        }

                        var result = next()(a1, a2, a3, a4, a5);

                        var @eventPost = new OnCommandExecuteHookEvent(ref command, HookMode.Post);
                        EventPublisher.InvokeOnCommandExecuteHook(@eventPost);
                        return result;
                    }
                    return next()(a1, a2, a3, a4, a5);
                }
            };
        });
    }

    internal void UnhookExecuteCommand()
    {
        if (executeCommand == null) return;
        executeCommand.RemoveHook(executeCommandGuid);
        executeCommand = null;
    }

    internal void WeaponDropPre( ref WeaponDropPreContext @event )
    {
        if (!EventPublisher.ListensToWeaponDrop) return;

        var @e = new OnWeaponServicesDropWeaponHook {
            WeaponServices = @event.Params.Player.PlayerPawn!.WeaponServices!,
            Weapon = @event.Params.Weapon,
            SwappingWeapon = @event.Params.SwappingWeapon,
            Result = @event.HookResult
        };
        EventPublisher.InvokeOnWeaponServicesDropWeaponHook(@e);
        @event.SetHookResult(@e.Result);
    }

    internal void HookWeaponServicesDropWeapon()
    {
        core.GameHooks.Weapons.Drop.Pre += WeaponDropPre;
    }

    internal void UnhookWeaponServicesDropWeapon()
    {
        core.GameHooks.Weapons.Drop.Pre -= WeaponDropPre;
    }

    internal void HookICvarFindConCommandTemplate()
    {
        var offset = core.GameData.GetOffset("ICvar::FindConCommand");
        if (IsWindows)
        {
            findConCommandWindows = core.Memory.GetUnmanagedFunctionByVTable<ICvarFindConCommandWindows>(core.Memory.GetVTableAddress(Library.Tier0, "CCvar")!.Value, offset);
            logger.LogInformation("Hooking ICvar::FindConCommand at {Address:X}", findConCommandWindows.Address);
            findConCommandGuid = findConCommandWindows.AddHook(( next ) =>
            {
                return ( pICvar, pRet, pConCommandName, unk1 ) =>
                {
                    var commandName = Marshal.PtrToStringAnsi(pConCommandName)!;
                    if (commandName.StartsWith("ecwb", StringComparison.OrdinalIgnoreCase))
                    {
                        commandName = commandName.Substring(4);
                        var bytes = Encoding.UTF8.GetBytes(commandName);
                        unsafe
                        {
                            var pStr = (nint)NativeMemory.AllocZeroed((nuint)bytes.Length);
                            pStr.CopyFrom(bytes);
                            var result = next()(pICvar, pRet, pStr, unk1);
                            NativeMemory.Free((void*)pStr);
                            return result;
                        }
                    }
                    return next()(pICvar, pRet, pConCommandName, unk1);
                };
            });
        }
        else
        {
            findConCommandLinux = core.Memory.GetUnmanagedFunctionByVTable<ICvarFindConCommandLinux>(core.Memory.GetVTableAddress(Library.Tier0, "CCvar")!.Value, offset);
            logger.LogInformation("Hooking ICvar::FindConCommand at {Address:X}", findConCommandLinux.Address);
            findConCommandGuid = findConCommandLinux.AddHook(( next ) =>
            {
                return ( pICvar, pConCommandName, unk1 ) =>
                {
                    var commandName = Marshal.PtrToStringUTF8(pConCommandName)!;
                    if (commandName.StartsWith("ecwb", StringComparison.OrdinalIgnoreCase))
                    {
                        commandName = commandName.Substring(4);
                        var bytes = Encoding.UTF8.GetBytes(commandName);
                        unsafe
                        {
                            var pStr = (nint)NativeMemory.AllocZeroed((nuint)bytes.Length);
                            pStr.CopyFrom(bytes);
                            var result = next()(pICvar, pStr, unk1);
                            NativeMemory.Free((void*)pStr);
                            return result;
                        }
                    }
                    return next()(pICvar, pConCommandName, unk1);
                };
            });
        }
    }

    internal void UnhookICvarFindConCommandTemplate()
    {
        if (IsWindows)
        {
            if (findConCommandWindows == null) return;
            findConCommandWindows.RemoveHook(findConCommandGuid);
            findConCommandWindows = null;
        }
        else
        {
            if (findConCommandLinux == null) return;
            findConCommandLinux.RemoveHook(findConCommandGuid);
            findConCommandLinux = null;
        }
    }

    internal void CanAcquireEventPost( ref CanAcquireItemPostContext @event )
    {
        if (!EventPublisher.ListensToCanAcquire) return;

        var @e = new OnItemServicesCanAcquireHookEvent {
            ItemServices = @event.Params.Player.PlayerPawn!.ItemServices!,
            EconItemView = @event.Params.EconItemView,
            WeaponVData = @event.Params.WeaponVData,
            AcquireMethod = @event.Params.AcquireMethod,
            OriginalResult = @event.Return
        };

        EventPublisher.InvokeOnCanAcquireHook(@e);
        @event.Return = @e.OriginalResult;
    }

    internal void HookCCSPlayerItemServicesCanAcquire()
    {
        core.GameHooks.Items.CanAcquire.Post += CanAcquireEventPost;
    }

    internal void UnhookCCSPlayerItemServicesCanAcquire()
    {
        core.GameHooks.Items.CanAcquire.Post -= CanAcquireEventPost;
    }

    internal void CanUseEventPost( ref CanUseWeaponPostContext @event )
    {
        if (!EventPublisher.ListensToCanUseWeapon) return;

        var @e = new OnWeaponServicesCanUseHookEvent {
            WeaponServices = @event.Params.Player.PlayerPawn!.WeaponServices!,
            Weapon = @event.Params.Weapon,
            OriginalResult = @event.Return
        };

        EventPublisher.InvokeOnWeaponServicesCanUseHook(@e);
        @event.Return = @e.OriginalResult;
    }

    internal void HookCCSPlayerWeaponServicesCanUse()
    {
        core.GameHooks.Weapons.CanUse.Post += CanUseEventPost;
    }

    internal void UnhookCCSPlayerWeaponServicesCanUse()
    {
        core.GameHooks.Weapons.CanUse.Post -= CanUseEventPost;
    }

    internal void EntityStartTouchPre( ref StartTouchEntityPreContext @event )
    {
        if (!EventPublisher.ListensToEntityStartTouch) return;

        using var @e = new OnEntityStartTouchEvent {
            Entity = @event.Params.Entity,
            OtherEntity = @event.Params.OtherEntity
        };
        EventPublisher.InvokeOnEntityStartTouch(@e);
    }

    internal void EntityTouchPre( ref TouchEntityPreContext @event )
    {
        if (!EventPublisher.ListensToEntityTouch) return;

        using var @e = new OnEntityTouchEvent {
            Entity = @event.Params.Entity,
            OtherEntity = @event.Params.OtherEntity
        };
        EventPublisher.InvokeOnEntityTouch(@e);
    }

    internal void EntityEndTouchPre( ref EndTouchEntityPreContext @event )
    {
        if (!EventPublisher.ListensToEntityEndTouch) return;

        using var @e = new OnEntityEndTouchEvent {
            Entity = @event.Params.Entity,
            OtherEntity = @event.Params.OtherEntity
        };
        EventPublisher.InvokeOnEntityEndTouch(@e);
    }

    internal void HookCBaseEntityTouchTemplate()
    {
        core.GameHooks.Entities.StartTouch.Pre += EntityStartTouchPre;
        core.GameHooks.Entities.Touch.Pre += EntityTouchPre;
        core.GameHooks.Entities.EndTouch.Pre += EntityEndTouchPre;
    }

    internal void UnhookCBaseEntityTouchTemplate()
    {
        core.GameHooks.Entities.StartTouch.Pre -= EntityStartTouchPre;
        core.GameHooks.Entities.Touch.Pre -= EntityTouchPre;
        core.GameHooks.Entities.EndTouch.Pre -= EntityEndTouchPre;
    }

    internal void HookSteamServerAPIActivated()
    {
        var offset = core.GameData.GetOffset("IServerGameDLL::GameServerSteamAPIActivated");
        steamServerAPIActivated = core.Memory.GetUnmanagedFunctionByVTable<SteamServerAPIActivated>(core.Memory.GetVTableAddress(Library.Server, "CSource2Server")!.Value, offset);
        logger.LogInformation("Hooking IServerGameDLL::GameServerSteamAPIActivated at {Address:X}", steamServerAPIActivated.Address);
        steamServerAPIActivatedGuid = steamServerAPIActivated.AddHook(next =>
        {
            return ( pServer ) =>
            {
                if (!CSteamGameServerAPIContext.Init())
                {
                    logger.LogError("Failed to initialize Steamworks GameServer API context.");
                    return;
                }

                EventPublisher.InvokeOnSteamAPIActivatedHook();
                next()(pServer);
            };
        });
    }

    internal void UnhookSteamServerAPIActivated()
    {
        if (steamServerAPIActivated == null) return;
        steamServerAPIActivated.RemoveHook(steamServerAPIActivatedGuid);
        steamServerAPIActivated = null;
    }

    internal void MovementServicesRunCommandHookPre( ref RunCommandMovementPreContext @event )
    {
        if (!EventPublisher.ListensToRunCommand) return;

        using var @ev = new OnMovementServicesRunCommandHookEvent {
            MovementServices = @event.Params.Player.PlayerPawn!.MovementServices!,
            ButtonState = @event.Params.UserCmd.ButtonState,
            UserCmdPB = @event.Params.UserCmd.CSGOUserCmd
        };
        EventPublisher.InvokeOnMovementServicesRunCommandHook(@ev);
    }

    internal void HookCPlayerMovementServicesRunCommand()
    {
        core.GameHooks.Movement.RunCommand.Pre += MovementServicesRunCommandHookPre;
    }

    internal void UnhookCPlayerMovementServicesRunCommand()
    {
        core.GameHooks.Movement.RunCommand.Pre -= MovementServicesRunCommandHookPre;
    }

    internal void CCSPlayerPostPostThinkPre( ref PostThinkPawnPreContext @event )
    {
        if (!EventPublisher.ListensToPostThink) return;

        using var @ev = new OnPlayerPawnPostThinkHookEvent {
            PlayerPawn = @event.Params.Player.PlayerPawn!
        };
        EventPublisher.InvokeOnPlayerPawnPostThinkHook(@ev);
    }

    internal void HookCCSPlayerPawnPostThink()
    {
        core.GameHooks.Pawn.PostThink.Pre += CCSPlayerPostPostThinkPre;
    }

    internal void UnhookCCSPlayerPawnPostThink()
    {
        core.GameHooks.Pawn.PostThink.Pre -= CCSPlayerPostPostThinkPre;
    }

    internal void HookDispatchDatamapFunction()
    {
        var address = core.GameData.GetSignature("DispatchDatamapFunction");
        dispatchDatamapFunction = core.Memory.GetUnmanagedFunctionByAddress<DispatchDatamapFunction>(address);
        dispatchDatamapFunctionGuid = dispatchDatamapFunction.AddHook(next =>
        {
            return ( a1, pDatamapFunc, a3, a4, a5, a6 ) =>
            {
                try
                {
                    var func = pDatamapFunc;
                    if (DatamapFunctionHookManager.TryGetHook(func, out var hook))
                    {
                        func = hook;
                    }
                    next()(a1, func, a3, a4, a5, a6);
                }
                catch (Exception e)
                {
                    if (!GlobalExceptionHandler.Handle(ref e)) return;
                    AnsiConsole.WriteException(e);
                }
            };
        });
    }

    internal void UnhookDispatchDatamapFunction()
    {
        if (dispatchDatamapFunction == null) return;
        dispatchDatamapFunction.RemoveHook(dispatchDatamapFunctionGuid);
        dispatchDatamapFunction = null;
    }

    internal void OnClientProcessUsercmds( ref ProcessUsercmdsPreContext @event )
    {
        if (!EventPublisher.ListensToProcessUsercmds) return;

        var v = new List<CSGOUserCmdPB>(@event.Params.Usercmds.Count);
        foreach (var usercmd in @event.Params.Usercmds)
        {
            v.Add(usercmd.CSGOUserCmd);
        }

        var @ev = new OnClientProcessUsercmdsEvent {
            PlayerId = @event.Params.Player.PlayerID,
            Paused = @event.Params.Paused,
            Margin = @event.Params.Margin,
            Usercmds = v
        };
        EventPublisher.OnClientProcessUsercmds(ref @ev);
    }

    internal void HookOnClientProcessUsercmds()
    {
        core.GameHooks.Controller.ProcessUsercmds.Pre += OnClientProcessUsercmds;
    }

    internal void UnhookOnClientProcessUsercmds()
    {
        core.GameHooks.Controller.ProcessUsercmds.Pre -= OnClientProcessUsercmds;
    }

    internal unsafe void EntityTakeDamagePre( ref TakeDamageEntityPreContext @event )
    {
        if (!EventPublisher.ListensToTakeDamage) return;

        var @ev = new OnEntityTakeDamageEvent {
            Entity = @event.Params.Entity,
            _infoPtr = (nint)@event.Params._infoPtr,
            _resultPtr = (nint)@event.Params.DamageResult
        };
        EventPublisher.InvokeOnEntityTakeDamage(ref @ev);
        @event.SetHookResult(@ev.Result);
    }

    internal void HookCBaseEntityTakeDamage()
    {
        core.GameHooks.Entities.TakeDamage.Pre += EntityTakeDamagePre;
    }

    internal void UnhookCBaseEntityTakeDamage()
    {
        core.GameHooks.Entities.TakeDamage.Pre -= EntityTakeDamagePre;
    }

    public void Dispose()
    {
        UnhookExecuteCommand();
        UnhookICvarFindConCommandTemplate();
        UnhookCCSPlayerItemServicesCanAcquire();
        UnhookCCSPlayerWeaponServicesCanUse();
        UnhookCBaseEntityTouchTemplate();
        UnhookSteamServerAPIActivated();
        UnhookEntityIdentityAcceptInput();
        UnhookEntityIOOutputFireOutputInternal();
        UnhookWeaponServicesDropWeapon();
        UnhookDispatchDatamapFunction();
        UnhookCCSPlayerPawnPostThink();
        UnhookCPlayerMovementServicesRunCommand();
        UnhookOnClientProcessUsercmds();
        UnhookCBaseEntityTakeDamage();
    }
}
