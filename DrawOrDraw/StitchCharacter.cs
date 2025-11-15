using System.Collections.Generic;
using Godot;
public partial class StitchCharacter : Node2D

{
    [Export] private Sprite2D Head, Torso, LeftUpperArm, LeftForearm, RightUpperArm, RightForearm, LeftThigh, LeftShin, RightThigh, RightShin;
    [Export] private Node2D Neck, LeftShoulder, LeftElbow, RightShoulder, RightElbow, LeftHip, LeftKnee, RightHip, RightKnee;
    public Dictionary<textName, SpriteArray2D> bodyParts;
    public void SetTextures(Dictionary<textName, SpriteArray2D> parts)
    {
        
        foreach(var (key, part) in parts)
        {
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
                    LeftForearm.Texture = ImageTexture.CreateFromImage(part.CreateImage());
                    RightForearm.Texture = ImageTexture.CreateFromImage(part.CreateImage());
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