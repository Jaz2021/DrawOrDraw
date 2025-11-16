using System;
using Godot;
using Networking_V2;

public partial class StageScene : Scene
{
    private StitchCharacter p1, p2;
    [Export] private Node2D p1Start, p2Start;
    [Export] private Camera2D cam;
    public override void _Ready()
    {
        ReadyPacket.ReadyPacketReceived += ReadyPacketReceived;
    }
    public override void _Process(double delta)
    {
        cam.GlobalPosition = (p1.GlobalPosition + p2.GlobalPosition) * 0.5f;
    }
    private void ReadyPacketReceived(ReadyPacket packet, ConnectionManager connection)
    {
        
    }

    public void SpawnStitchedChars(StitchCharacter p1, StitchCharacter p2)
    {
        this.p1 = p1;
        this.p2 = p2;
        GD.Print("Spawning stitched charas");
        objectRoot.CallDeferred("add_child", p1);
        objectRoot.CallDeferred("add_child", p2);
        // if(NetworkingV2.IsLobbyOwner())
        // {
            p1.Init((ulong)NetworkingV2.steamID, ObjectType.Player, objectRoot, Vector2.Zero);
            foreach(var lobbyMember in NetworkingV2.GetLobbyMembers())
            {
                if(lobbyMember.steamID == NetworkingV2.steamID)
                {
                    continue;
                }
                p2.Init((ulong)lobbyMember.steamID, ObjectType.Player, objectRoot, Vector2.Zero);
                break;
            }

        // } else
        // {
            // p2.Init((ulong)NetworkingV2.steamID, ObjectType.Player, objectRoot, Vector2.Zero);
            // foreach(var lobbyMember in NetworkingV2.GetLobbyMembers())
            // {
            //     if(lobbyMember.steamID == NetworkingV2.steamID)
            //     {
            //         continue;
            //     }
            //     p1.Init((ulong)lobbyMember.steamID, ObjectType.Player, objectRoot, Vector2.Zero);
            //     break;
            // }
        // }
        if (NetworkingV2.IsLobbyOwner())
        {
            p1.GlobalPosition = p1Start.GlobalPosition;
            playerController.SetPlayerObject(p1);
            p2.GlobalPosition = p2Start.GlobalPosition;
        } else
        {
            p2.GlobalPosition = p1Start.GlobalPosition;
            playerController.SetPlayerObject(p1);

            p1.GlobalPosition = p2Start.GlobalPosition;
        }
        
    }
}