using Godot;

namespace Networking_V2;
[Packet]
public partial class SpawnObjectPacket : IPacket<SpawnObjectPacket>
{
    [SerializeData]
    private byte objtype;
    public ObjectType ObjType
    {
        get
        {
            return (ObjectType)objtype;
        }
    }
    [SerializeData]
    public Vector2 position;
    [SerializeData]
    public ulong id;
}