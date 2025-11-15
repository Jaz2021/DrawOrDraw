using System;
using Godot;
using Networking_V2;

public partial class CanvasScene : Scene
{
    
    public override void _Ready()
    {

    }


    public override void SpawnObject(ObjectType type, ulong id, Vector2 position, bool send = false)
    {
        // Do nothing extra for this :)
    }
    public override void EnterScene(ObjectStructList objectSpawns, Vector2 position)
    {
        // Do nothing extra for this
    }
    protected override void SpawnObjectPacketReceived(SpawnObjectPacket packet, ConnectionManager connection)
    {
        // We don't actually care what the other player is doing yet here
    }
}