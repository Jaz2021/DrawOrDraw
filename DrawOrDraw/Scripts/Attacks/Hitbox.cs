using Godot;
[GlobalClass]
public partial class Hitbox : Resource
{
    [Export]
    public Vector2 position;
    [Export]
    public Vector2 size;
    [Export]
    public float startTime;
    [Export]
    public float endTime;
}