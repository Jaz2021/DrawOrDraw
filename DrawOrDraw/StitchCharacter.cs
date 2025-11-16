using System.Collections.Generic;
using Godot;
using Networking_V2;

public struct Animation
{
    private float startTime;
	private float endTime;
	private float jointRotations;
}
public partial class StitchCharacter : NetObject

{
	[Export] private Sprite2D Head, Torso, LeftUpperArm, LeftForearm, RightUpperArm, RightForearm, LeftThigh, LeftShin, RightThigh, RightShin;
	[Export] private Node2D Neck, LeftShoulder, LeftElbow, RightShoulder, RightElbow, LeftHip, LeftKnee, RightHip, RightKnee;
	public Dictionary<textName, SpriteArray2D> bodyParts = new();
	private ulong id;
	public void Init(ulong id)
	{
		this.id = id;
	}
	public enum Attacks
	{
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
		ForwardSpecial,
		NoAttack
	}

	public enum PlayerState
	{
		Idle,
		Dead,
		Attacking,
		Moving
	}

	public override void _Ready()
	{
		PlayerPacket.PlayerPacketReceived += PlayerPacketReceived;
	}
	private void PlayerPacketReceived(PlayerPacket packet, ConnectionManager connection)
	{
		// GD.Print($"Received player packet with id: {packet.id}, my id: {id}");
		if (connection != null)
		{
			if (id == (ulong)NetworkingV2.steamID && connection.steamID != NetworkingV2.GetLobbyOwner())
			{
				// If you are receiving for your own player and it is coming from not the lobby owner, ignore it
				return;
			}
		}
		// Received packet from myself or the lobby owner, so listen to it
		if (id == packet.id)
		{
			pos = packet.position;
			vel = packet.velocity;
			ReceivedUpdate = true;
		}
	}

	public void SetTextures(Dictionary<textName, SpriteArray2D> parts)
	{
		GD.Print("Setting tetures");
		foreach (var (key, part) in parts)
		{
			switch (key)
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
