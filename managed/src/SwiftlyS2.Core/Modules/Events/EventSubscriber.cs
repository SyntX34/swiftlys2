using Microsoft.Extensions.Logging;
using SwiftlyS2.Core.GameHooks;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Profiler;

namespace SwiftlyS2.Core.Events;

/// <summary>
/// Plugin-scoped custom event subscriber.
/// </summary>
internal class EventSubscriber : IEventSubscriber, IDisposable
{
    private readonly IContextedProfilerService profiler;
    private readonly ILogger<EventSubscriber> logger;

    private volatile bool disposed;

    public bool Disposed => disposed;

    public EventSubscriber( IContextedProfilerService profiler, ILogger<EventSubscriber> logger )
    {
        this.profiler = profiler;
        this.logger = logger;
        this.disposed = false;
        EventPublisher.Subscribe(this);
    }

    ~EventSubscriber()
    {
        Dispose();
    }

    public event EventDelegates.OnTick? OnTick;
    public event EventDelegates.OnWorldUpdate? OnWorldUpdate;
    public event EventDelegates.OnClientConnected? OnClientConnected;
    public event EventDelegates.OnClientDisconnected? OnClientDisconnected;
    public event EventDelegates.OnClientKeyStateChanged? OnClientKeyStateChanged;
    public event EventDelegates.OnClientPutInServer? OnClientPutInServer;
    public event EventDelegates.OnClientSteamAuthorize? OnClientSteamAuthorize;
    public event EventDelegates.OnClientSteamAuthorizeFail? OnClientSteamAuthorizeFail;
    public event EventDelegates.OnEntityCreated? OnEntityCreated;
    public event EventDelegates.OnEntityDeleted? OnEntityDeleted;
    public event EventDelegates.OnEntityParentChanged? OnEntityParentChanged;
    public event EventDelegates.OnEntitySpawned? OnEntitySpawned;
    public event EventDelegates.OnMapLoad? OnMapLoad;
    public event EventDelegates.OnMapUnload? OnMapUnload;
    private EventDelegates.OnClientProcessUsercmds? _OnClientProcessUsercmds;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Controller.ProcessUsercmds instead.")]
    public event EventDelegates.OnClientProcessUsercmds? OnClientProcessUsercmds
    {
        add
        {
            if (_OnClientProcessUsercmds == null) GameHooksPublisher.AddHookListener(HookListener.ProcessUsercmds);
            _OnClientProcessUsercmds += value;
        }
        remove
        {
            _OnClientProcessUsercmds -= value;
            if (_OnClientProcessUsercmds == null) GameHooksPublisher.RemoveHookListener(HookListener.ProcessUsercmds);
        }
    }
    public event EventDelegates.OnConVarValueChanged? OnConVarValueChanged;
    public event EventDelegates.OnConCommandCreated? OnConCommandCreated;
    public event EventDelegates.OnConVarCreated? OnConVarCreated;
    private EventDelegates.OnEntityTakeDamage? _OnEntityTakeDamage;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Entities.TakeDamage instead.")]
    public event EventDelegates.OnEntityTakeDamage? OnEntityTakeDamage
    {
        add
        {
            if (_OnEntityTakeDamage == null) GameHooksPublisher.AddHookListener(HookListener.TakeDamage);
            _OnEntityTakeDamage += value;
        }
        remove
        {
            _OnEntityTakeDamage -= value;
            if (_OnEntityTakeDamage == null) GameHooksPublisher.RemoveHookListener(HookListener.TakeDamage);
        }
    }
    public event EventDelegates.OnPrecacheResource? OnPrecacheResource;
    private EventDelegates.OnEntityStartTouch? _OnEntityStartTouch;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Entities.StartTouch instead.")]
    public event EventDelegates.OnEntityStartTouch? OnEntityStartTouch
    {
        add
        {
            if (_OnEntityStartTouch == null) GameHooksPublisher.AddHookListener(HookListener.StartTouch);
            _OnEntityStartTouch += value;
        }
        remove
        {
            _OnEntityStartTouch -= value;
            if (_OnEntityStartTouch == null) GameHooksPublisher.RemoveHookListener(HookListener.StartTouch);
        }
    }
    private EventDelegates.OnEntityTouch? _OnEntityTouch;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Entities.Touch instead.")]
    public event EventDelegates.OnEntityTouch? OnEntityTouch
    {
        add
        {
            if (_OnEntityTouch == null) GameHooksPublisher.AddHookListener(HookListener.Touch);
            _OnEntityTouch += value;
        }
        remove
        {
            _OnEntityTouch -= value;
            if (_OnEntityTouch == null) GameHooksPublisher.RemoveHookListener(HookListener.Touch);
        }
    }
    private EventDelegates.OnEntityEndTouch? _OnEntityEndTouch;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Entities.EndTouch instead.")]
    public event EventDelegates.OnEntityEndTouch? OnEntityEndTouch
    {
        add
        {
            if (_OnEntityEndTouch == null) GameHooksPublisher.AddHookListener(HookListener.EndTouch);
            _OnEntityEndTouch += value;
        }
        remove
        {
            _OnEntityEndTouch -= value;
            if (_OnEntityEndTouch == null) GameHooksPublisher.RemoveHookListener(HookListener.EndTouch);
        }
    }
    private EventDelegates.OnItemServicesCanAcquireHook? _OnItemServicesCanAcquireHook;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Items.CanAcquire instead.")]
    public event EventDelegates.OnItemServicesCanAcquireHook? OnItemServicesCanAcquireHook
    {
        add
        {
            if (_OnItemServicesCanAcquireHook == null) GameHooksPublisher.AddHookListener(HookListener.CanAcquire);
            _OnItemServicesCanAcquireHook += value;
        }
        remove
        {
            _OnItemServicesCanAcquireHook -= value;
            if (_OnItemServicesCanAcquireHook == null) GameHooksPublisher.RemoveHookListener(HookListener.CanAcquire);
        }
    }
    private EventDelegates.OnWeaponServicesCanUseHook? _OnWeaponServicesCanUseHook;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Weapons.CanUse instead.")]
    public event EventDelegates.OnWeaponServicesCanUseHook? OnWeaponServicesCanUseHook
    {
        add
        {
            if (_OnWeaponServicesCanUseHook == null) GameHooksPublisher.AddHookListener(HookListener.CanUse);
            _OnWeaponServicesCanUseHook += value;
        }
        remove
        {
            _OnWeaponServicesCanUseHook -= value;
            if (_OnWeaponServicesCanUseHook == null) GameHooksPublisher.RemoveHookListener(HookListener.CanUse);
        }
    }
    private EventDelegates.OnWeaponServicesDropWeaponHook? _OnWeaponServicesDropWeaponHook;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Weapons.Drop instead.")]
    public event EventDelegates.OnWeaponServicesDropWeaponHook? OnWeaponServicesDropWeaponHook
    {
        add
        {
            if (_OnWeaponServicesDropWeaponHook == null) GameHooksPublisher.AddHookListener(HookListener.WeaponDrop);
            _OnWeaponServicesDropWeaponHook += value;
        }
        remove
        {
            _OnWeaponServicesDropWeaponHook -= value;
            if (_OnWeaponServicesDropWeaponHook == null) GameHooksPublisher.RemoveHookListener(HookListener.WeaponDrop);
        }
    }
    public event EventDelegates.OnConsoleOutput? OnConsoleOutput;
    public event EventDelegates.OnCommandExecuteHook? OnCommandExecuteHook;
    public event EventDelegates.OnSteamAPIActivated? OnSteamAPIActivated;
    private EventDelegates.OnMovementServicesRunCommandHook? _OnMovementServicesRunCommandHook;

    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Movement.RunCommand instead.")]
    public event EventDelegates.OnMovementServicesRunCommandHook? OnMovementServicesRunCommandHook
    {
        add
        {
            if (_OnMovementServicesRunCommandHook == null) GameHooksPublisher.AddHookListener(HookListener.RunCommand);
            _OnMovementServicesRunCommandHook += value;
        }
        remove
        {
            _OnMovementServicesRunCommandHook -= value;
            if (_OnMovementServicesRunCommandHook == null) GameHooksPublisher.RemoveHookListener(HookListener.RunCommand);
        }
    }
    private EventDelegates.OnPlayerPawnPostThink? _OnPlayerPawnPostThink;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Pawn.PostThink instead.")]
    public event EventDelegates.OnPlayerPawnPostThink? OnPlayerPawnPostThink
    {
        add
        {
            if (_OnPlayerPawnPostThink == null) GameHooksPublisher.AddHookListener(HookListener.PostThink);
            _OnPlayerPawnPostThink += value;
        }
        remove
        {
            _OnPlayerPawnPostThink -= value;
            if (_OnPlayerPawnPostThink == null) GameHooksPublisher.RemoveHookListener(HookListener.PostThink);
        }
    }
    private EventDelegates.OnEntityIdentityAcceptInputHook? _OnEntityIdentityAcceptInputHook;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Entities.AcceptInput instead.")]
    public event EventDelegates.OnEntityIdentityAcceptInputHook? OnEntityIdentityAcceptInputHook
    {
        add
        {
            if (_OnEntityIdentityAcceptInputHook == null) GameHooksPublisher.AddHookListener(HookListener.AcceptInput);
            _OnEntityIdentityAcceptInputHook += value;
        }
        remove
        {
            _OnEntityIdentityAcceptInputHook -= value;
            if (_OnEntityIdentityAcceptInputHook == null) GameHooksPublisher.RemoveHookListener(HookListener.AcceptInput);
        }
    }
    private EventDelegates.OnEntityFireOutputHookEvent? _OnEntityFireOutputHook;
    [Obsolete("This event is deprecated and will be removed in future versions. Use GameHooks.Entities.FireOutput instead.")]
    public event EventDelegates.OnEntityFireOutputHookEvent? OnEntityFireOutputHook
    {
        add
        {
            if (_OnEntityFireOutputHook == null) GameHooksPublisher.AddHookListener(HookListener.FireOutput);
            _OnEntityFireOutputHook += value;
        }
        remove
        {
            _OnEntityFireOutputHook -= value;
            if (_OnEntityFireOutputHook == null) GameHooksPublisher.RemoveHookListener(HookListener.FireOutput);
        }
    }
    public event EventDelegates.OnStartupServer? OnStartupServer;
    public event EventDelegates.OnClientVoice? OnClientVoice;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }
        disposed = true;

        if (_OnClientProcessUsercmds != null) GameHooksPublisher.RemoveHookListener(HookListener.ProcessUsercmds);
        if (_OnEntityTakeDamage != null) GameHooksPublisher.RemoveHookListener(HookListener.TakeDamage);
        if (_OnEntityStartTouch != null) GameHooksPublisher.RemoveHookListener(HookListener.StartTouch);
        if (_OnEntityTouch != null) GameHooksPublisher.RemoveHookListener(HookListener.Touch);
        if (_OnEntityEndTouch != null) GameHooksPublisher.RemoveHookListener(HookListener.EndTouch);
        if (_OnItemServicesCanAcquireHook != null) GameHooksPublisher.RemoveHookListener(HookListener.CanAcquire);
        if (_OnWeaponServicesCanUseHook != null) GameHooksPublisher.RemoveHookListener(HookListener.CanUse);
        if (_OnWeaponServicesDropWeaponHook != null) GameHooksPublisher.RemoveHookListener(HookListener.WeaponDrop);
        if (_OnPlayerPawnPostThink != null) GameHooksPublisher.RemoveHookListener(HookListener.PostThink);
        if (_OnEntityIdentityAcceptInputHook != null) GameHooksPublisher.RemoveHookListener(HookListener.AcceptInput);
        if (_OnEntityFireOutputHook != null) GameHooksPublisher.RemoveHookListener(HookListener.FireOutput);
        if (_OnMovementServicesRunCommandHook != null) GameHooksPublisher.RemoveHookListener(HookListener.RunCommand);

        EventPublisher.Unsubscribe(this);
        GC.SuppressFinalize(this);
    }

    public bool ListensToTick => OnTick != null;

    public void InvokeOnTick()
    {
        if (OnTick == null)
        {
            return;
        }

        try
        {
            profiler.StartRecording("Event::OnTick");
            OnTick.Invoke();
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnTick.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnTick");
        }
    }

    public bool ListensToWorldUpdate => OnWorldUpdate != null;

    public void InvokeOnWorldUpdate()
    {
        if (OnWorldUpdate == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnWorldUpdate");
            OnWorldUpdate.Invoke();
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnWorldUpdate.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnWorldUpdate");
        }
    }

    public bool ListensToClientConnected => OnClientConnected != null;

    public void InvokeOnClientConnected( ref OnClientConnectedEvent @event )
    {
        if (OnClientConnected == null)
        {
            return;
        }

        try
        {
            profiler.StartRecording("Event::OnClientConnected");
            OnClientConnected.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientConnected.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientConnected");
        }
    }

    public bool ListensToClientDisconnected => OnClientDisconnected != null;

    public void InvokeOnClientDisconnected( ref OnClientDisconnectedEvent @event )
    {
        if (OnClientDisconnected == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnClientDisconnected");
            OnClientDisconnected.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientDisconnected.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientDisconnected");
        }
    }

    public bool ListensToClientKeyStateChanged => OnClientKeyStateChanged != null;

    public void InvokeOnClientKeyStateChanged( ref OnClientKeyStateChangedEvent @event )
    {
        if (OnClientKeyStateChanged == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnClientKeyStateChanged");
            OnClientKeyStateChanged.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientKeyStateChanged.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientKeyStateChanged");
        }
    }

    public bool ListensToClientPutInServer => OnClientPutInServer != null;

    public void InvokeOnClientPutInServer( ref OnClientPutInServerEvent @event )
    {
        if (OnClientPutInServer == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnClientPutInServer");
            OnClientPutInServer.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientPutInServer.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientPutInServer");
        }
    }

    public bool ListensToClientSteamAuthorize => OnClientSteamAuthorize != null;

    public void InvokeOnClientSteamAuthorize( ref OnClientSteamAuthorizeEvent @event )
    {
        if (OnClientSteamAuthorize == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnClientSteamAuthorize");
            OnClientSteamAuthorize.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientSteamAuthorize.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientSteamAuthorize");
        }
    }

    public bool ListensToClientSteamAuthorizeFail => OnClientSteamAuthorizeFail != null;

    public void InvokeOnClientSteamAuthorizeFail( ref OnClientSteamAuthorizeFailEvent @event )
    {
        if (OnClientSteamAuthorizeFail == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnClientSteamAuthorizeFail");
            OnClientSteamAuthorizeFail.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientSteamAuthorizeFail.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientSteamAuthorizeFail");
        }
    }

    public bool ListensToEntityCreated => OnEntityCreated != null;

    public void InvokeOnEntityCreated( ref OnEntityCreatedEvent @event )
    {
        if (OnEntityCreated == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityCreated");
            OnEntityCreated.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityCreated.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityCreated");
        }
    }

    public bool ListensToClientVoice => OnClientVoice != null;

    public void InvokeOnClientVoice( ref OnClientVoiceEvent @event )
    {
        if (OnClientVoice == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnClientVoice");
            OnClientVoice.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientVoice.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientVoice");
        }
    }

    public bool ListensToEntityDeleted => OnEntityDeleted != null;

    public void InvokeOnEntityDeleted( ref OnEntityDeletedEvent @event )
    {
        if (OnEntityDeleted == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityDeleted");
            OnEntityDeleted.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityDeleted.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityDeleted");
        }
    }

    public bool ListensToEntityParentChanged => OnEntityParentChanged != null;

    public void InvokeOnEntityParentChanged( ref OnEntityParentChangedEvent @event )
    {
        if (OnEntityParentChanged == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityParentChanged");
            OnEntityParentChanged.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityParentChanged.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityParentChanged");
        }
    }

    public bool ListensToEntitySpawned => OnEntitySpawned != null;

    public void InvokeOnEntitySpawned( ref OnEntitySpawnedEvent @event )
    {
        if (OnEntitySpawned == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntitySpawned");
            OnEntitySpawned.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntitySpawned.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntitySpawned");
        }
    }

    public bool ListensToMapLoad => OnMapLoad != null;

    public void InvokeOnMapLoad( ref OnMapLoadEvent @event )
    {
        if (OnMapLoad == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnMapLoad");
            OnMapLoad.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnMapLoad.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnMapLoad");
        }
    }

    public bool ListensToMapUnload => OnMapUnload != null;

    public void InvokeOnMapUnload( ref OnMapUnloadEvent @event )
    {
        if (OnMapUnload == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnMapUnload");
            OnMapUnload.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnMapUnload.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnMapUnload");
        }
    }

    public bool ListensToProcessUsercmds => _OnClientProcessUsercmds != null;

    public bool ListensToTakeDamage => _OnEntityTakeDamage != null;

    public void InvokeOnClientProcessUsercmds( ref OnClientProcessUsercmdsEvent @event )
    {
        if (_OnClientProcessUsercmds == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnClientProcessUsercmds");
            _OnClientProcessUsercmds.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnClientProcessUsercmds.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnClientProcessUsercmds");
        }
    }

    public void InvokeOnEntityTakeDamage( ref OnEntityTakeDamageEvent @event )
    {
        if (_OnEntityTakeDamage == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityTakeDamage");
            _OnEntityTakeDamage.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityTakeDamage.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityTakeDamage");
        }
    }

    public bool ListensToPrecacheResource => OnPrecacheResource != null;

    public void InvokeOnPrecacheResource( ref OnPrecacheResourceEvent @event )
    {
        if (OnPrecacheResource == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnPrecacheResource");
            OnPrecacheResource.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnPrecacheResource.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnPrecacheResource");
        }
    }

    public bool ListensToEntityStartTouch => _OnEntityStartTouch != null;

    public void InvokeOnEntityStartTouch( ref OnEntityStartTouchEvent @event )
    {
        if (_OnEntityStartTouch == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityStartTouch");
            _OnEntityStartTouch.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityStartTouch.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityStartTouch");
        }
    }

    public bool ListensToEntityTouch => _OnEntityTouch != null;

    public void InvokeOnEntityTouch( ref OnEntityTouchEvent @event )
    {
        if (_OnEntityTouch == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityTouch");
            _OnEntityTouch.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityTouch.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityTouch");
        }
    }

    public bool ListensToEntityEndTouch => _OnEntityEndTouch != null;

    public void InvokeOnEntityEndTouch( ref OnEntityEndTouchEvent @event )
    {
        if (_OnEntityEndTouch == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityEndTouch");
            _OnEntityEndTouch.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityEndTouch.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityEndTouch");
        }
    }

    public void InvokeOnSteamAPIActivatedHook()
    {
        if (OnSteamAPIActivated == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnSteamAPIActivatedHook");
            OnSteamAPIActivated.Invoke();
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnSteamAPIActivatedHook.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnSteamAPIActivatedHook");
        }
    }

    public bool ListensToCanAcquire => _OnItemServicesCanAcquireHook != null;

    public void InvokeOnItemServicesCanAcquireHook( ref OnItemServicesCanAcquireHookEvent @event )
    {
        if (_OnItemServicesCanAcquireHook == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnItemServicesCanAcquireHook");
            _OnItemServicesCanAcquireHook.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnItemServicesCanAcquireHook.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnItemServicesCanAcquireHook");
        }
    }

    public bool ListensToCanUseWeapon => _OnWeaponServicesCanUseHook != null;

    public void InvokeOnWeaponServicesCanUseHook( ref OnWeaponServicesCanUseHookEvent @event )
    {
        if (_OnWeaponServicesCanUseHook == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnWeaponServicesCanUseHook");
            _OnWeaponServicesCanUseHook.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnWeaponServicesCanUseHook.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnWeaponServicesCanUseHook");
        }
    }

    public bool ListensToConsoleOutput => OnConsoleOutput != null;

    public void InvokeOnConsoleOutput( ref OnConsoleOutputEvent @event )
    {
        if (OnConsoleOutput == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnConsoleOutput");
            OnConsoleOutput.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnConsoleOutput.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnConsoleOutput");
        }
    }

    public bool ListensToConVarValueChanged => OnConVarValueChanged != null;

    public void InvokeOnConVarValueChanged( ref OnConVarValueChanged @event )
    {
        if (OnConVarValueChanged == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnConVarValueChanged");
            OnConVarValueChanged.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnConVarValueChanged.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnConVarValueChanged");
        }
    }

    public bool ListensToConCommandCreated => OnConCommandCreated != null;

    public void InvokeOnConCommandCreated( ref OnConCommandCreated @event )
    {
        if (OnConCommandCreated == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnConCommandCreated");
            OnConCommandCreated.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnConCommandCreated.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnConCommandCreated");
        }
    }

    public bool ListensToConVarCreated => OnConVarCreated != null;

    public void InvokeOnConVarCreated( ref OnConVarCreated @event )
    {
        if (OnConVarCreated == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnConVarCreated");
            OnConVarCreated.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnConVarCreated.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnConVarCreated");
        }
    }

    public void InvokeOnCommandExecuteHook( ref OnCommandExecuteHookEvent @event )
    {
        if (OnCommandExecuteHook == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnCommandExecuteHook");
            OnCommandExecuteHook.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnCommandExecuteHook.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnCommandExecuteHook");
        }
    }

    public bool ListensToRunCommand => _OnMovementServicesRunCommandHook != null;

    public void InvokeOnMovementServicesRunCommandHook( ref OnMovementServicesRunCommandHookEvent @event )
    {
        if (_OnMovementServicesRunCommandHook == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnMovementServicesRunCommandHook");
            _OnMovementServicesRunCommandHook.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnMovementServicesRunCommandHook.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnMovementServicesRunCommandHook");
        }
    }

    public bool ListensToPostThink => _OnPlayerPawnPostThink != null;

    public void InvokeOnPlayerPawnPostThinkHook( ref OnPlayerPawnPostThinkHookEvent @event )
    {
        if (_OnPlayerPawnPostThink == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnPlayerPawnPostThink");
            _OnPlayerPawnPostThink.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnPlayerPawnPostThink.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnPlayerPawnPostThink");
        }
    }

    public bool ListensToAcceptInput => _OnEntityIdentityAcceptInputHook != null;

    public void InvokeOnEntityIdentityAcceptInputHook( ref OnEntityIdentityAcceptInputHookEvent @event )
    {
        if (_OnEntityIdentityAcceptInputHook == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityIdentityAcceptInput");
            _OnEntityIdentityAcceptInputHook.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityIdentityAcceptInput.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityIdentityAcceptInput");
        }
    }

    public bool ListensToWeaponDrop => _OnWeaponServicesDropWeaponHook != null;

    public void InvokeOnWeaponServicesDropWeaponHook( ref OnWeaponServicesDropWeaponHook @event )
    {
        if (_OnWeaponServicesDropWeaponHook == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnWeaponServicesDropWeaponHook");
            _OnWeaponServicesDropWeaponHook.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnWeaponServicesDropWeaponHook.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnWeaponServicesDropWeaponHook");
        }
    }

    public bool ListensToFireOutput => _OnEntityFireOutputHook != null;

    public void InvokeOnEntityFireOutputHook( ref OnEntityFireOutputHookEvent @event )
    {
        if (_OnEntityFireOutputHook == null)
        {
            return;
        }
        try
        {
            profiler.StartRecording("Event::OnEntityFireOutputHook");
            _OnEntityFireOutputHook.Invoke(@event);
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnEntityFireOutputHook.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnEntityFireOutputHook");
        }
    }

    public bool ListensToStartupServer => OnStartupServer != null;

    public void InvokeOnStartupServer()
    {
        if (OnStartupServer == null)
        {
            return;
        }

        try
        {
            profiler.StartRecording("Event::OnStartupServer");
            OnStartupServer.Invoke();
        }
        catch (Exception e)
        {
            if (GlobalExceptionHandler.Handle(ref e))
            {
                logger.LogError(e, "Error invoking OnStartupServer.");
            }
        }
        finally
        {
            profiler.StopRecording("Event::OnStartupServer");
        }
    }
}
