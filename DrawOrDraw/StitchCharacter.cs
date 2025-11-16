using System.Collections.Generic;
using Godot;
using Networking_V2;
public partial class StitchCharacter : NetObject
{
	[Export] private int stock = 3;
	[Export] private float RotationSpeed = 0.05f;
	[Export] private float throwVelocity = 10f;
	[Export] private Sprite2D Head, Torso, LeftUpperArm, LeftForearm, RightUpperArm, RightForearm, LeftThigh, LeftShin, RightThigh, RightShin;
	[Export] private Node2D Neck, LeftShoulder, LeftElbow, RightShoulder, RightElbow, LeftHip, LeftKnee, RightHip, RightKnee;
	[Export] private PackedScene VictoryScene, DefeatScene;
	public Dictionary<textName, SpriteArray2D> bodyParts = new();
	

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
	[Export] private CollisionShape2D headCollider;
	[Export] private PackedScene headScene;
	private Head head;
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
	public bool headThrown = false;
    public override void _Process(double delta)
    {
        RotateLegsRight(delta);
		FlailArms();
    }
	public void Throw(Vector2 direction)
    {
        if (headThrown)
        {
            return;
        }
		headThrown = true;
        head.GlobalPosition = GlobalPosition;
		head.Velocity = throwVelocity * direction;
		head.Visible = true;
		Head.Visible = false;
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
        if (!Head.Visible)
        {
            return; // Ignore if our head isn't actually there
        }
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
		stock--;
		if (stock <= 0)
        {
            GD.Print("You lost the game loser lmao");
			QueueFree();
			Globals.Instance.ChangeScene(DefeatScene, Vector2.Zero);
			
        } else
        {
           	GlobalPosition = Vector2.Zero;
			Velocity = Vector2.Zero;
        }
    }
	public void PickupHead()
    {
		headThrown = false;
        Head.Visible = true;
		head.Visible = false;
    }
	public override void _PhysicsProcess(double delta)
    {
        Grounded = IsOnGround();
        if (ReceivedUpdate)
        {
            ReceivedUpdate = false;
            Position = pos; // Need to test if this needs to be inverted
        }
        if(Grounded)
        {
            Velocity = new(Mathf.Clamp(Velocity.X, -MaxGroundSpeed, MaxGroundSpeed), Velocity.Y);
        } else
        {
            Velocity = new(Mathf.Clamp(Velocity.X, -MaxAirSpeed, MaxAirSpeed), Velocity.Y + (Gravity * (float)delta * gravMult));
            // GD.Print($"Airborne: {Position}");
        }
        MoveAndSlide();
        if(Grounded)
        {
            Velocity = new(0f, Velocity.Y);
        }
        // GD.Print($"Packet id: {id}");
        if(id == (ulong)NetworkingV2.steamID)
        {
            PlayerPacket packet = new(headThrown, id, GlobalPosition, Velocity);
            NetworkingV2.SendPacketToAll(packet);
        }
        
        // GD.Print("Sending packet");
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
					head = headScene.Instantiate<Head>();
					head.SetTexture(part, id);
					head.Visible = false;
					RectangleShape2D shape = new();
					var coords = part.FindMaxMinCoords();
					shape.Size = new(coords.Y - coords.X, coords.W - coords.Z);
					headCollider.Shape = shape;
					Globals.Instance.currentScene.objectRoot.CallDeferred("add_child", head);
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
