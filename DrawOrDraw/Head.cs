using Godot;
using System;

public partial class Head : CharacterBody2D
{
    [Export] private CollisionShape2D collider;
    [Export] private Sprite2D sprite;
    public void SetTexture(SpriteArray2D s)
    {
        sprite.Texture = ImageTexture.CreateFromImage(s.CreateImage());
    }
}
