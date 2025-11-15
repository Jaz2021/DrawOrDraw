namespace Networking_V2;
[Packet]
public partial class ReadyPacket : IPacket<ReadyPacket>
{
    // This shouldn't actually need anything in it, we are just looking to receive a ready packet saying we can send over our image
}