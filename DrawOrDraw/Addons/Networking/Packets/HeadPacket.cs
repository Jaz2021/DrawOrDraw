using Godot;

namespace Networking_V2;
[Packet]
public partial class HeadPacket : IPacket<HeadPacket>
{
    [SerializeData]
    public ulong id;
    [SerializeData]
    public Vector2 position;
    [SerializeData]
    public Vector2 velocity;
    [SerializeData]
    public bool visible;
}