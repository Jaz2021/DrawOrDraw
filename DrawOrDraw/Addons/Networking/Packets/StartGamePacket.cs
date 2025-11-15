using Godot;

namespace Networking_V2;
[Packet]
public partial class StartGamePacket : IPacket<StartGamePacket>
{
    [SerializeData]
    public Vector2 StartPosition;
    [SerializeData]
    public ObjectStructList ExistingObjects;
}