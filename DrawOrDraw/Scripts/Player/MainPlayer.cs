using Godot;
using Networking_V2;
using Steamworks;
using System;

public partial class MainPlayer : Node
{
    private StitchCharacter myObj;
    [Export] private float groundAccel;
    [Export] private float airAccel;
    [Export] private float JumpForce;
    [Export] private int jumps = 3;
    private int jumpCount = 0;
    // private float gravMult = 1f;
    private Vector2 moveDir = Vector2.Zero;
    
    public void SetPlayerObject(StitchCharacter pobj)
    {
        myObj = pobj; // Set the player object that we update every tick
        Input.MouseMode = Input.MouseModeEnum.Captured;
        // myObj.PhysicsMaterialOverride = new();
        // myObj.PhysicsMaterialOverride.Rough = true;
    }

    public override void _Process(double delta)
    {
        if (myObj == null)
        {
            return;
        }

        // Handle our inputs
        Vector2 inputDir = Vector2.Zero;
        // if (!myObj.Grounded)
        // {
        //     return;
        // }
        if (Input.IsActionPressed("MoveForward"))
        {
            // GD.Print("Forward pressed");

            inputDir.Y = -1f;
            // moveDir.Z -= moveSpeedForward * (float)delta;
        }
        if (Input.IsActionPressed("MoveBack"))
        {
            inputDir.Y = 1f;
            myObj.gravMult = 30f;
            // moveDir.Z += moveSpeedForward * (float)delta;
        } else
        {
            myObj.gravMult = 1f;
        }
        if (Input.IsActionPressed("MoveLeft"))
        {
            inputDir.X = -1f;
            // moveDir.X -= moveSpeedLR * (float)delta;
        }
        if (Input.IsActionPressed("MoveRight"))
        {
            inputDir.X = 1f;
            // moveDir.X += moveSpeedLR * (float)delta;
        }
        

        inputDir = inputDir.Normalized();
        if(myObj.Grounded)
        {
            inputDir = new(inputDir.X * (float)delta * groundAccel, 0f);
        } else
        {
            inputDir = new(inputDir.X * (float)delta * airAccel, 0f);
        }
        moveDir += inputDir;
        // GD.Print($"Movedir: {moveDir}, velocity: {myObj.LinearVelocity}");
        
    }
    public override void _Input(InputEvent e)
    {
        if(e is InputEventMouseMotion m)
        {
            return;
        }
        if (e.IsActionPressed("Interact"))
        {
            myObj.Throw(moveDir.Normalized());
        }
        if (e.IsActionPressed("Pause"))
        {
            if(Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            } else
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if(myObj == null)
        {
            return;
        }
        if (Input.IsActionJustPressed("Jump"))
        {
            // GD.Print("Checking if on ground");
            if (myObj.Grounded && moveDir.Y == 0)
            {
                moveDir.Y = -JumpForce;
                jumpCount = 0;
            } else if(jumpCount < jumps)
            {
                moveDir.Y = -JumpForce;
                jumpCount += 1;
            }
        }
        myObj.Velocity += moveDir;
        moveDir = Vector2.Zero;
        SendPacket();
    }
    private void SendPacket()
    {
        if(NetworkingV2.isInit && NetworkingV2.GetLobbyID() != (CSteamID)0)
        {
            PlayerPacket packet = new(myObj.headThrown, myObj.id, myObj.Position, myObj.Velocity);
            NetworkingV2.SendPacketToAll(packet);
        }
    }
}
