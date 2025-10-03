#pragma warning disable CS0649
#pragma warning disable CS0169

using System.Buffers;
using System.Text;
using System.Threading;
using SwiftlyS2.Shared.Natives;

namespace SwiftlyS2.Core.Natives;

internal static class NativeHooks {
  private static int _MainThreadID;
  private unsafe static delegate* unmanaged<nint> _AllocateHook;
  public unsafe static nint AllocateHook() {
    var ret = _AllocateHook();
    return ret;
  }
  private unsafe static delegate* unmanaged<nint> _AllocateVHook;
  public unsafe static nint AllocateVHook() {
    var ret = _AllocateVHook();
    return ret;
  }
  private unsafe static delegate* unmanaged<nint> _AllocateMidHook;
  public unsafe static nint AllocateMidHook() {
    var ret = _AllocateMidHook();
    return ret;
  }
  private unsafe static delegate* unmanaged<nint, void> _DeallocateHook;
  public unsafe static void DeallocateHook(nint hook) {
    _DeallocateHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, void> _DeallocateVHook;
  public unsafe static void DeallocateVHook(nint hook) {
    _DeallocateVHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, void> _DeallocateMidHook;
  public unsafe static void DeallocateMidHook(nint hook) {
    _DeallocateMidHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, nint, nint, void> _SetHook;
  /// <summary>
  /// the callback should receive the exact arguments as the function has, and to return the same amount of arguments
  /// </summary>
  public unsafe static void SetHook(nint hook, nint func, nint callback) {
    _SetHook(hook, func, callback);
  }
  private unsafe static delegate* unmanaged<nint, nint, int, nint, bool, void> _SetVHook;
  /// <summary>
  /// the callback should receive the exact arguments as the function has, and to return the same amount of arguments, plus the first argument needs to be the pointer to the original function
  /// </summary>
  public unsafe static void SetVHook(nint hook, nint entityOrVTable, int index, nint callback, bool isVtable) {
    _SetVHook(hook, entityOrVTable, index, callback, isVtable);
  }
  private unsafe static delegate* unmanaged<nint, nint, nint, void> _SetMidHook;
  public unsafe static void SetMidHook(nint hook, nint instr, nint callback) {
    _SetMidHook(hook, instr, callback);
  }
  private unsafe static delegate* unmanaged<nint, void> _EnableHook;
  public unsafe static void EnableHook(nint hook) {
    _EnableHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, void> _EnableVHook;
  public unsafe static void EnableVHook(nint hook) {
    _EnableVHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, void> _EnableMidHook;
  public unsafe static void EnableMidHook(nint hook) {
    _EnableMidHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, void> _DisableHook;
  public unsafe static void DisableHook(nint hook) {
    _DisableHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, void> _DisableVHook;
  public unsafe static void DisableVHook(nint hook) {
    _DisableVHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, void> _DisableMidHook;
  public unsafe static void DisableMidHook(nint hook) {
    _DisableMidHook(hook);
  }
  private unsafe static delegate* unmanaged<nint, bool> _IsHookEnabled;
  public unsafe static bool IsHookEnabled(nint hook) {
    var ret = _IsHookEnabled(hook);
    return ret;
  }
  private unsafe static delegate* unmanaged<nint, bool> _IsVHookEnabled;
  public unsafe static bool IsVHookEnabled(nint hook) {
    var ret = _IsVHookEnabled(hook);
    return ret;
  }
  private unsafe static delegate* unmanaged<nint, bool> _IsMidHookEnabled;
  public unsafe static bool IsMidHookEnabled(nint hook) {
    var ret = _IsMidHookEnabled(hook);
    return ret;
  }
  private unsafe static delegate* unmanaged<nint, nint> _GetHookOriginal;
  public unsafe static nint GetHookOriginal(nint hook) {
    var ret = _GetHookOriginal(hook);
    return ret;
  }
  private unsafe static delegate* unmanaged<nint, nint> _GetVHookOriginal;
  public unsafe static nint GetVHookOriginal(nint hook) {
    var ret = _GetVHookOriginal(hook);
    return ret;
  }
}
