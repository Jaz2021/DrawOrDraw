using System;
using Godot;
using Networking_V2;

public partial class WaitingScene : Scene
{
    [Export] private PackedScene CanvasScene;
    public override void _Ready()
    {
        BeginPacket.BeginPacketReceived += BeginPacketReceived;   
    }
    public void BeginGame()
    {
        GD.Print("Begin game pressed");
        BeginPacketReceived(null, null);
    }
    private void BeginPacketReceived(BeginPacket packet, ConnectionManager connection)
    {
        if (NetworkingV2.IsLobbyOwner())
        {
            // If we are the lobby owner and we receive a BeginPacket, that means everyone is ready, we can start drawing.
            BeginPacket b = new();
            NetworkingV2.SendPacketToAll(b, true);
        }
        Globals.Instance.ChangeScene(CanvasScene, Vector2.Zero);
    }
    public override void EnterScene(ObjectStructList objectSpawns, Vector2 position)
    {
        
    }
}
