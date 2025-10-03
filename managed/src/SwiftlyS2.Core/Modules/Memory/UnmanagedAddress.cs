using Microsoft.Extensions.Logging;
using SwiftlyS2.Core.Hooks;
using SwiftlyS2.Shared.Memory;
using SwiftlyS2.Shared.Natives;
using System;
using System.Runtime.InteropServices;

namespace SwiftlyS2.Core.Memory;

internal class UnmanagedAddress : IUnmanagedAddress, IDisposable
{
    public nint Address { get; private set; }

    public List<Guid> Hooks { get; } = new();

    private HookManager _HookManager { get; set; }

    private ILogger<UnmanagedAddress> _Logger { get; set; }

    public UnmanagedAddress(nint address, HookManager hookManager, ILoggerFactory loggerFactory)
    {
        _Logger = loggerFactory.CreateLogger<UnmanagedAddress>();
        _HookManager = hookManager;
        Address = address;
    }

    public Guid AddHook(Action<IMidHook> callback)
    {
        MidHookCallback mdCallback = (ref MidHookContext context) =>
        {

        };
    }

    public void RemoveHook(Guid id)
    {
    }

    public void Dispose()
    {
    }
}