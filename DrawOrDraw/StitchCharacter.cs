using Godot;
public partial class StitchCharacter : Node2D

{
    [Export] private Sprite2D Head, Torso, LeftUpperArm, LeftForearm, RightUpperArm, RightForearm, LeftThigh, LeftShin, RightThigh, RightShin;
    [Export] private Node2D Neck, LeftShoulder, LeftElbow, RightShoulder, RightElbow, LeftHip, LeftKnee, RightHip, RightKnee;
    public void SetTextures(bodyPart[] parts)
    {
        foreach(var part in parts)
        {
            switch(part.spriteName)
            {
                case textName.head:
                    Head.Texture = ImageTexture.CreateFromImage(part.texture);
                    break;
                case textName.lower_arm:
                    LeftForearm.Texture = ImageTexture.CreateFromImage(part.texture);
                    RightForearm.Texture = ImageTexture.CreateFromImage(part.texture);
                    break;
                case textName.upper_arm:
                    LeftForearm.Texture = ImageTexture.CreateFromImage(part.texture);
                    RightForearm.Texture = ImageTexture.CreateFromImage(part.texture);
                    break;
                case textName.thigh:
                    LeftThigh.Texture = ImageTexture.CreateFromImage(part.texture);
                    RightThigh.Texture = ImageTexture.CreateFromImage(part.texture);
                    break;
                case textName.shin:
                    LeftShin.Texture = ImageTexture.CreateFromImage(part.texture);
                    RightShin.Texture = ImageTexture.CreateFromImage(part.texture);
                    break;
                case textName.torso:
                    Torso.Texture = ImageTexture.CreateFromImage(part.texture);
                    break;
            }
        }
    }
}