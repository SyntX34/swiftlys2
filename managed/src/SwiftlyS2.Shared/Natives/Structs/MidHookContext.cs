using System.Drawing;
using System.Runtime.InteropServices;

namespace SwiftlyS2.Shared.Natives;

/// <summary>
/// Delegate for MidHook callback functions.
/// </summary>
/// <param name="ctx">Reference to the MidHookContext containing register state.</param>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void MidHookCallback(ref MidHookContext ctx);

/// <summary>
/// Union representing XMM register data in various formats.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 16)]
public unsafe struct Xmm
{
    [FieldOffset(0)]
    public fixed byte U8[16];

    [FieldOffset(0)]
    public fixed ushort U16[8];

    [FieldOffset(0)]
    public fixed uint U32[4];

    [FieldOffset(0)]
    public fixed ulong U64[2];

    [FieldOffset(0)]
    public fixed float F32[4];

    [FieldOffset(0)]
    public fixed double F64[2];
}

/// <summary>
/// Context structure for 64-bit MidHook.
/// </summary>
/// <remarks>
/// This structure is used to pass the context of the hooked function to the destination allowing full access
/// to the 64-bit registers at the moment the hook is called.
/// <para>rip will point to a trampoline containing the replaced instruction(s).</para>
/// <para>rsp is read-only. Modifying it will have no effect. Use trampoline_rsp to modify rsp if needed but make sure
/// the top of the stack is the rip you want to resume at.</para>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public struct MidHookContext
{
    public Xmm Xmm0;
    public Xmm Xmm1;
    public Xmm Xmm2;
    public Xmm Xmm3;
    public Xmm Xmm4;
    public Xmm Xmm5;
    public Xmm Xmm6;
    public Xmm Xmm7;
    public Xmm Xmm8;
    public Xmm Xmm9;
    public Xmm Xmm10;
    public Xmm Xmm11;
    public Xmm Xmm12;
    public Xmm Xmm13;
    public Xmm Xmm14;
    public Xmm Xmm15;

    public nuint RFlags;
    public nuint R15;
    public nuint R14;
    public nuint R13;
    public nuint R12;
    public nuint R11;
    public nuint R10;
    public nuint R9;
    public nuint R8;
    public nuint Rdi;
    public nuint Rsi;
    public nuint Rdx;
    public nuint Rcx;
    public nuint Rbx;
    public nuint Rax;
    public nuint Rbp;
    public nuint Rsp;
    public nuint TrampolineRsp;
    public nuint Rip;
}