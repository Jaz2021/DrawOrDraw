// var test = Image.createFromData(32, 32, false, Image.Format.Rgb8, [0, 0, 0, 0, 0, 0, 0])

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

public struct SpritePixel
{
    public byte r = 255;
    public byte g = 255;
    public byte b = 255;
    public byte a = 0;

    public SpritePixel()
    {
    }

    public byte[] Serialize()
    {
        return [r, g, b, a];
    }
}

public struct SpriteArray2D
{
    public SpritePixel[][] Pixels;
    public SpriteArray2D(int size)
    {
        Pixels = new SpritePixel[size][];
        for (int i = 0; i < size; i++)
        {
            Pixels[i] = new SpritePixel[size];
            for (int j = 0; j < size; j++)
            {
                Pixels[i][j] = new();
            }
        }
    }
    public byte[] Serialize()
    {
        List<byte> bytes = new();

        foreach (var column in Pixels)
        {
            foreach (var pixel in column)
            {
                bytes.AddRange(pixel.Serialize());
            }
        }
        return bytes.ToArray();
    }
    public Image CreateImage()
    {
        return Image.CreateFromData(Pixels[0].Length, Pixels.Length, false, Image.Format.Rgba8, Serialize());
    }

    public int findArea()
    {
        int maxX, minX, maxY, minY;
        maxX = minX = maxY = minY = 0;
        for (int i = 0; i < Pixels.Length; i++)
        {
            for (int j = 0; i < Pixels[0].Length; i++)
            {
                if (Pixels[i][j].a != 0)
                {
                    if (i < minX)
                    {
                        minX = i;
                    }

                    if (i >= maxX)
                    {
                        maxX = i;
                    }

                    if (j < minY)
                    {
                        minY = j;
                    }

                    if (j >= maxY)
                    {
                        maxY = j;
                    }
                }
            }
        }

        int deltaX = maxX - minX;
        int deltaY = maxY - minY;
        return deltaX * deltaY;
    }
}