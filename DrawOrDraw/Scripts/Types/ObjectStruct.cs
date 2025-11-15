using System.Collections.Generic;
using Godot;

public struct ObjectStruct
{
    public ObjectStruct(ulong id, Vector2 position, ObjectType objectType)
    {
        this.id = id;
        this.position = position;
        objType = objectType;
    }
    public readonly ulong id;
    public readonly Vector2 position;
    public readonly ObjectType objType;
    public byte[] Serialize()
    {
        return [
            ..id.Serialize(),
            ..position.Serialize(),
            (byte)objType
        ];
    }
}
public class ObjectStructList
{
    public readonly ObjectStruct[] objs;
    public ObjectStructList(ObjectStruct[] objs)
    {
        this.objs = objs;
    }
    public byte[] Serialize()
    {
        List<byte> data = new();
        data.AddRange(objs.Length.Serialize());
        foreach (var obj in objs)
        {
            data.AddRange(obj.Serialize());
        }
        return data.ToArray();
    }
}
public enum ObjectType : byte
{
    Player
}