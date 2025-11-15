namespace Networking_V2;
using Godot;
[Packet]
public partial class PlayerPacket : IPacket<PlayerPacket>
{
    [SerializeData]
    public Vector2 position;
    [SerializeData]
    public Vector2 velocity;
    [SerializeData]
    public ulong id;
}