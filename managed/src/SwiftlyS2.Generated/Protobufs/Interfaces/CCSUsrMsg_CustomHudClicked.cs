using SwiftlyS2.Core.ProtobufDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.NetMessages;

namespace SwiftlyS2.Shared.ProtobufDefinitions;

public interface CCSUsrMsg_CustomHudClicked : ITypedProtobuf<CCSUsrMsg_CustomHudClicked>, INetMessage<CCSUsrMsg_CustomHudClicked>, IDisposable
{
    static int INetMessage<CCSUsrMsg_CustomHudClicked>.MessageId => 390;

    static string INetMessage<CCSUsrMsg_CustomHudClicked>.MessageName => "CCSUsrMsg_CustomHudClicked";

    static CCSUsrMsg_CustomHudClicked ITypedProtobuf<CCSUsrMsg_CustomHudClicked>.Wrap(nint handle, bool isManuallyAllocated) => new CCSUsrMsg_CustomHudClickedImpl(handle, isManuallyAllocated);

    public uint CustomHudLayout { get; set; }
    public string ButtonId { get; set; }
}