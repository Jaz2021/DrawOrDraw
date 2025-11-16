using Godot;
using Networking_V2;

public partial class NetObject : CharacterBody2D
{
    protected bool ReceivedUpdate = false;
    protected Vector2 pos;
    protected Vector2 vel;
    [Export] private float JumpRayLength; // Debug
    [Export] private float MaxGroundSpeed = 5f;
    [Export] private float MaxAirSpeed = 4.5f;
    [Export] private RayCast2D FootPos; // Suggested to use a Marker3D to show the position
    [Export] private float Gravity = -98.81f;
    public ObjectType type
    {
        get;
        private set;
    }
    public ulong id
    {
        get;
        protected set;
    }
    public bool Grounded
    {
        get;
        protected set;
    }
    protected Node2D objRoot;
    public void Init(ulong id, ObjectType type, Node2D objRoot, Vector2 position)
    {
        GD.Print($"Initiallizing NetObj with id {id}");
        this.id = id;
        this.objRoot = objRoot;
        pos = position;
        this.type = type;
    }
    public override void _Ready()
    {
        base._Ready();
        PlayerPacket.PlayerPacketReceived += ReceivedPacket;
        GD.Print("Listening for Player packets");
    }
    private void ReceivedPacket(PlayerPacket packet, ConnectionManager connection)
    {
        GD.Print($"Received packet for pid: {packet.id}");
        // if(packet.id == id)
        // {
        
        Velocity = packet.velocity;
        Position = packet.position;
        // }
    }

    protected bool IsOnGround()
    {
        FootPos.ForceRaycastUpdate();
        return FootPos.IsColliding() && Velocity.Y <= 0.001f;
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
            Velocity = new(Mathf.Clamp(Velocity.X, -MaxAirSpeed, MaxAirSpeed), Velocity.Y + (Gravity * (float)delta));
            // GD.Print($"Airborne: {Position}");
        }
        MoveAndSlide();
        if(Grounded)
        {
            Velocity = new(0f, Velocity.Y);
        }
        PlayerPacket packet = new(id, GlobalPosition, Velocity);
        NetworkingV2.SendPacketToAll(packet);
        // GD.Print("Sending packet");
    }
}