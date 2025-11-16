namespace Networking_V2;
[Packet]
public partial class WinGamePacket : IPacket<WinGamePacket>
{}
// This is sent whenever one loses. If you receive it you have immediately won the game