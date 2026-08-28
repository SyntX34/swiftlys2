#pragma warning disable CS0649
#pragma warning disable CS0169

using System.Buffers;
using System.Text;
using System.Threading;
using SwiftlyS2.Shared.Natives;

namespace SwiftlyS2.Core.Natives;

internal static class NativeDatabase
{

    private unsafe static delegate* unmanaged<int*, byte*> _GetDefaultDriver;

    public unsafe static string GetDefaultDriver()
    {
        var length = 0;
        var returnedPtr = _GetDefaultDriver(&length);
        var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
        NativeAllocator.Free((nint)returnedPtr);
        return outString;
    }

    private unsafe static delegate* unmanaged<int*, byte*> _GetDefaultConnectionName;

    public unsafe static string GetDefaultConnectionName()
    {
        var length = 0;
        var returnedPtr = _GetDefaultConnectionName(&length);
        var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
        NativeAllocator.Free((nint)returnedPtr);
        return outString;
    }

    private unsafe static delegate* unmanaged<int*, byte*, byte*> _GetConnectionDriver;

    public unsafe static string GetConnectionDriver(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var length = 0;
            var returnedPtr = _GetConnectionDriver(&length, connectionNameBufferPtr);
            var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
            NativeAllocator.Free((nint)returnedPtr);
            return outString;
        }
    }

    private unsafe static delegate* unmanaged<int*, byte*, byte*> _GetConnectionHost;

    public unsafe static string GetConnectionHost(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var length = 0;
            var returnedPtr = _GetConnectionHost(&length, connectionNameBufferPtr);
            var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
            NativeAllocator.Free((nint)returnedPtr);
            return outString;
        }
    }

    private unsafe static delegate* unmanaged<int*, byte*, byte*> _GetConnectionDatabase;

    public unsafe static string GetConnectionDatabase(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var length = 0;
            var returnedPtr = _GetConnectionDatabase(&length, connectionNameBufferPtr);
            var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
            NativeAllocator.Free((nint)returnedPtr);
            return outString;
        }
    }

    private unsafe static delegate* unmanaged<int*, byte*, byte*> _GetConnectionUser;

    public unsafe static string GetConnectionUser(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var length = 0;
            var returnedPtr = _GetConnectionUser(&length, connectionNameBufferPtr);
            var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
            NativeAllocator.Free((nint)returnedPtr);
            return outString;
        }
    }

    private unsafe static delegate* unmanaged<int*, byte*, byte*> _GetConnectionPass;

    public unsafe static string GetConnectionPass(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var length = 0;
            var returnedPtr = _GetConnectionPass(&length, connectionNameBufferPtr);
            var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
            NativeAllocator.Free((nint)returnedPtr);
            return outString;
        }
    }

    private unsafe static delegate* unmanaged<byte*, uint> _GetConnectionTimeout;

    public unsafe static uint GetConnectionTimeout(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var ret = _GetConnectionTimeout(connectionNameBufferPtr);
            return ret;
        }
    }

    private unsafe static delegate* unmanaged<byte*, ushort> _GetConnectionPort;

    public unsafe static ushort GetConnectionPort(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var ret = _GetConnectionPort(connectionNameBufferPtr);
            return ret;
        }
    }

    private unsafe static delegate* unmanaged<int*, byte*, byte*> _GetConnectionRawUri;

    public unsafe static string GetConnectionRawUri(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var length = 0;
            var returnedPtr = _GetConnectionRawUri(&length, connectionNameBufferPtr);
            var outString = StringAlloc.CreateCSharpString((nint)returnedPtr, length);
            NativeAllocator.Free((nint)returnedPtr);
            return outString;
        }
    }

    private unsafe static delegate* unmanaged<byte*, byte> _ConnectionExists;

    public unsafe static bool ConnectionExists(string connectionName)
    {
        using var connectionNameStr = new ScopedCString(connectionName);
        fixed (byte* connectionNameBufferPtr = connectionNameStr)
        {
            var ret = _ConnectionExists(connectionNameBufferPtr);
            return ret == 1;
        }
    }
}