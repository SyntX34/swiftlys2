#pragma warning disable CS0649
#pragma warning disable CS0169

using System.Buffers;
using System.Text;
using System.Threading;
using SwiftlyS2.Shared.Natives;

namespace SwiftlyS2.Core.Natives;

internal static class NativeEngineHelpers {
  private static int _MainThreadID;

  private unsafe static delegate* unmanaged<byte*, int> _GetServerIP;

  public unsafe static string GetServerIP() {
    var ret = _GetServerIP(null);
    var pool = ArrayPool<byte>.Shared;
    var retBuffer = pool.Rent(ret + 1);
    fixed (byte* retBufferPtr = retBuffer) {
      ret = _GetServerIP(retBufferPtr);
      var retString = Encoding.UTF8.GetString(retBufferPtr, ret);
      pool.Return(retBuffer);
      return retString;
    }
  }

  private unsafe static delegate* unmanaged<byte*, int> _GetMap;

  public unsafe static string GetMap() {
    var ret = _GetMap(null);
    var pool = ArrayPool<byte>.Shared;
    var retBuffer = pool.Rent(ret + 1);
    fixed (byte* retBufferPtr = retBuffer) {
      ret = _GetMap(retBufferPtr);
      var retString = Encoding.UTF8.GetString(retBufferPtr, ret);
      pool.Return(retBuffer);
      return retString;
    }
  }

  private unsafe static delegate* unmanaged<byte*, byte> _IsMapValid;

  /// <summary>
  /// it can be map name, or workshop id
  /// </summary>
  public unsafe static bool IsMapValid(string map_name) {
    var pool = ArrayPool<byte>.Shared;
    var map_nameLength = Encoding.UTF8.GetByteCount(map_name);
    var map_nameBuffer = pool.Rent(map_nameLength + 1);
    Encoding.UTF8.GetBytes(map_name, map_nameBuffer);
    map_nameBuffer[map_nameLength] = 0;
    fixed (byte* map_nameBufferPtr = map_nameBuffer) {
      var ret = _IsMapValid(map_nameBufferPtr);
      pool.Return(map_nameBuffer);
      return ret == 1;
    }
  }

  private unsafe static delegate* unmanaged<int> _GetMaxPlayers;

  public unsafe static int GetMaxPlayers() {
    var ret = _GetMaxPlayers();
    return ret;
  }

  private unsafe static delegate* unmanaged<byte*, void> _ExecuteCommand;

  public unsafe static void ExecuteCommand(string command) {
    var pool = ArrayPool<byte>.Shared;
    var commandLength = Encoding.UTF8.GetByteCount(command);
    var commandBuffer = pool.Rent(commandLength + 1);
    Encoding.UTF8.GetBytes(command, commandBuffer);
    commandBuffer[commandLength] = 0;
    fixed (byte* commandBufferPtr = commandBuffer) {
      _ExecuteCommand(commandBufferPtr);
      pool.Return(commandBuffer);
    }
  }

  private unsafe static delegate* unmanaged<float> _GetServerCurrentTime;

  public unsafe static float GetServerCurrentTime() {
    var ret = _GetServerCurrentTime();
    return ret;
  }

  private unsafe static delegate* unmanaged<int> _GetServerTickCount;

  /// <summary>
  /// simulation tick
  /// </summary>
  public unsafe static int GetServerTickCount() {
    var ret = _GetServerTickCount();
    return ret;
  }

  private unsafe static delegate* unmanaged<byte*, nint> _FindGameSystemByName;

  public unsafe static nint FindGameSystemByName(string name) {
    var pool = ArrayPool<byte>.Shared;
    var nameLength = Encoding.UTF8.GetByteCount(name);
    var nameBuffer = pool.Rent(nameLength + 1);
    Encoding.UTF8.GetBytes(name, nameBuffer);
    nameBuffer[nameLength] = 0;
    fixed (byte* nameBufferPtr = nameBuffer) {
      var ret = _FindGameSystemByName(nameBufferPtr);
      pool.Return(nameBuffer);
      return ret;
    }
  }

  private unsafe static delegate* unmanaged<byte*, void> _SendMessageToConsole;

  public unsafe static void SendMessageToConsole(string msg) {
    var pool = ArrayPool<byte>.Shared;
    var msgLength = Encoding.UTF8.GetByteCount(msg);
    var msgBuffer = pool.Rent(msgLength + 1);
    Encoding.UTF8.GetBytes(msg, msgBuffer);
    msgBuffer[msgLength] = 0;
    fixed (byte* msgBufferPtr = msgBuffer) {
      _SendMessageToConsole(msgBufferPtr);
      pool.Return(msgBuffer);
    }
  }

  private unsafe static delegate* unmanaged<nint> _GetTraceManager;

  public unsafe static nint GetTraceManager() {
    var ret = _GetTraceManager();
    return ret;
  }

  private unsafe static delegate* unmanaged<byte*, int> _GetCurrentGame;

  public unsafe static string GetCurrentGame() {
    var ret = _GetCurrentGame(null);
    var pool = ArrayPool<byte>.Shared;
    var retBuffer = pool.Rent(ret + 1);
    fixed (byte* retBufferPtr = retBuffer) {
      ret = _GetCurrentGame(retBufferPtr);
      var retString = Encoding.UTF8.GetString(retBufferPtr, ret);
      pool.Return(retBuffer);
      return retString;
    }
  }

  private unsafe static delegate* unmanaged<byte*, int> _GetNativeVersion;

  public unsafe static string GetNativeVersion() {
    var ret = _GetNativeVersion(null);
    var pool = ArrayPool<byte>.Shared;
    var retBuffer = pool.Rent(ret + 1);
    fixed (byte* retBufferPtr = retBuffer) {
      ret = _GetNativeVersion(retBufferPtr);
      var retString = Encoding.UTF8.GetString(retBufferPtr, ret);
      pool.Return(retBuffer);
      return retString;
    }
  }

  private unsafe static delegate* unmanaged<byte*, int> _GetMenuSettings;

  public unsafe static string GetMenuSettings() {
    var ret = _GetMenuSettings(null);
    var pool = ArrayPool<byte>.Shared;
    var retBuffer = pool.Rent(ret + 1);
    fixed (byte* retBufferPtr = retBuffer) {
      ret = _GetMenuSettings(retBufferPtr);
      var retString = Encoding.UTF8.GetString(retBufferPtr, ret);
      pool.Return(retBuffer);
      return retString;
    }
  }
}