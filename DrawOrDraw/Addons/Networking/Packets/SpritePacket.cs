namespace Networking_V2;
[Packet]
public partial class SpritePacket : IPacket<SpritePacket>
{
    [SerializeData]
    public SpriteArray2D headSprite;
    [SerializeData]
    public SpriteArray2D torsoSprite;
    [SerializeData]
    public SpriteArray2D thighSprite;
    [SerializeData]
    public SpriteArray2D shinSprite;
    [SerializeData]
    public SpriteArray2D upperArmSprite;
    [SerializeData]
    public SpriteArray2D forearmSprite;
}