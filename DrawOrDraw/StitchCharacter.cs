using System.Collections.Generic;
using Godot;
using Networking_V2;
public partial class StitchCharacter : NetObject

{
	[Export] private float RotationSpeed = 0.05f;
	[Export] private Sprite2D Head, Torso, LeftUpperArm, LeftForearm, RightUpperArm, RightForearm, LeftThigh, LeftShin, RightThigh, RightShin;
	[Export] private Node2D Neck, LeftShoulder, LeftElbow, RightShoulder, RightElbow, LeftHip, LeftKnee, RightHip, RightKnee;
	public Dictionary<textName, SpriteArray2D> bodyParts = new();
	public enum Attacks {
		NeutralTilt,
		ForwardTilt,
		DownTilt,
		UpTilt,
		NeutralAir,
		ForwardAir,
		UpAir,
		DownAir,
		UpSpecial,
		DownSpecial,
		NeutralSpecial,
		ForwardSpecial
	}

    // public override void _Ready()
    // {
    // 	PlayerPacket.PlayerPacketReceived += PlayerPacketReceived;
    // 	base._Ready();
    // }
    // private void PlayerPacketReceived(PlayerPacket packet, ConnectionManager connection)
    // {
    // 	// GD.Print($"Received player packet with id: {packet.id}, my id: {id}");
    // 	if (connection != null)
    // 	{
    // 		if (id == (ulong)NetworkingV2.steamID && connection.steamID != NetworkingV2.GetLobbyOwner())
    // 		{
    // 			// If you are receiving for your own player and it is coming from not the lobby owner, ignore it
    // 			return;
    // 		}
    // 	}
    // 	// Received packet from myself or the lobby owner, so listen to it
    // 	if (id == packet.id)
    // 	{
    // 		pos = packet.position;
    // 		vel = packet.velocity;
    // 		ReceivedUpdate = true;
    // 	}
    // }
    public override void _Process(double delta)
    {
        RotateLegsRight(delta);
		FlailArms();
    }
	public void RotateLegsRight(double delta)
    {
        RightHip.Rotate((float)delta * Velocity.X * RotationSpeed);
		LeftHip.Rotate((float)delta * Velocity.X * RotationSpeed);
		RightKnee.Rotate(-(float)delta * Velocity.X * RotationSpeed);
		LeftKnee.Rotate(-(float)delta * Velocity.X * RotationSpeed);
    }

	public void FlailArms()
    {
        LeftShoulder.Rotation = new Vector2(Velocity.X, -Velocity.Y).Angle();
		LeftElbow.Rotation = -Velocity.Angle();
		RightShoulder.Rotation = new Vector2(-Velocity.X, -Velocity.Y).Angle();
		RightElbow.Rotation = Velocity.Angle();
    }
	public void OnHeadEntered(Node2D node)
    {
        if(node is StitchCharacter s)
        {
            if(s != this && s.Velocity.Y >= 0)
            {
                s.Kill();
            }
        }
    }
	public void Kill()
    {
        GD.Print("Someone touched my head :(");
    }


	public void SetTextures(Dictionary<textName, SpriteArray2D> parts)
	{
		// GD.Print($"Setting tetures {parts.Count}");
		foreach(var (key, part) in parts)
		{
			// GD.Print(key);
			switch(key)
			{
				case textName.head:
					
					bodyParts[textName.head] = part;
					Head.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					break;
				case textName.lower_arm:
					bodyParts[textName.lower_arm] = part;
					LeftForearm.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					RightForearm.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					break;
				case textName.upper_arm:
					bodyParts[textName.upper_arm] = part;
					LeftUpperArm.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					RightUpperArm.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					break;
				case textName.thigh:
					bodyParts[textName.thigh] = part;
					LeftThigh.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					RightThigh.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					break;
				case textName.shin:
					bodyParts[textName.shin] = part;
					LeftShin.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					RightShin.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					break;
				case textName.torso:
					bodyParts[textName.torso] = part;
					Torso.Texture = ImageTexture.CreateFromImage(part.CreateImage());
					break;
			}
		}
	}
}
