using Godot;
using Networking_V2;
using System;
using System.Drawing;

public partial class Head : CharacterBody2D
{
    [Export] private CollisionShape2D collider;
    [Export] private Sprite2D sprite;
    [Export] private float gravity = 150f;
    private ulong id;
    StitchCharacter myObj;
    public override void _Ready()
    {
        HeadPacket.HeadPacketReceived += HeadPacketReceived;
    }

    private void HeadPacketReceived(HeadPacket packet, ConnectionManager connection)
    {
        if(id == packet.id)
        {
            GlobalPosition = packet.position;
            Velocity = packet.velocity;
            Visible = packet.visible;
        }
    }

    public void SetTexture(SpriteArray2D s, ulong id)
    {
        sprite.Texture = ImageTexture.CreateFromImage(s.CreateImage());
        var shape = new RectangleShape2D();
        var coords = s.FindMaxMinCoords();

        shape.Size = new(coords.Y - coords.X, coords.W - coords.Z);
        collider.Shape = shape;
        this.id = id;
    }
    public void ColliderEntered(Node2D obj)
    {
        if(!Visible)
        {
            return;
        }
        if (obj is StitchCharacter s)
        {
            if(s == myObj)
            {
                s.PickupHead();
            } else
            {
                if(s.Velocity.Y > 0)
                {
                    s.Kill();
                }
            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if(Visible)
        {
            Velocity += new Vector2(0f, gravity);
            MoveAndSlide();
        }
        HeadPacket packet = new(id, GlobalPosition, Velocity, Visible);
    }
}
