using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Godot;
using Steamworks;

public enum Pen
{
    pencil,
    eraser

};

public enum textName
{
    head,
    torso,
    upper_arm,
    lower_arm,
    thigh,
    shin
};

public enum penColor
{
    black,
    red,
    green,
    blue,
    purple,
    brown,
    grey,
    orange,
    yellow,
    pink,

};

public enum lineThickness
{
    one,
    three,
    five
};

public struct bodyPart
{
    public SpriteArray2D texture;
    public textName spriteName;
};

public partial class DrawingCanvas : Sprite2D
{
    [Export] private PackedScene stitchChar;
    [Export] private Sprite2D backgroundSprite;
    [Export] private Label label;
    private bodyPart[] drawnSprites;
    [Export] private Texture2D SelectedPencil, UnselectedPencil, SelectedEraser, UnselectedEraser;
    [Export] private Button PencilButton, EraserButton, BlackButton, RedButton, GreenButton, BlueButton, PurpleButton, BrownButton, GreyButton, OrangeButton, YellowButton, PinkButton;
    private SpriteArray2D canvas;
    private bool drawing = false;
    private Area2D[][] pixels;
    [Export] private float pixelSize = 1f;
    [Export] private int size = 48;
    private Pen _stylus;
    private penColor color;
    private lineThickness line;
    [Export] private Godot.Collections.Dictionary<textName, Texture2D> backgroundTextures;
    [Export] private Godot.Collections.Dictionary<textName, string> DrawMessage;
    [Export] private Godot.Collections.Array<textName> textNameOrder;
    private int currentTextName = 0;
    private Dictionary<textName, SpriteArray2D> sprites = new();
    private Pen Stylus
    {
        get => _stylus;
        set
        {
            _stylus = value;
            if (value == Pen.pencil)
            {
                PencilButton.Icon = SelectedPencil;
                EraserButton.Icon = UnselectedEraser;
            }
            else
            {
                EraserButton.Icon = SelectedEraser;
                PencilButton.Icon = UnselectedPencil;
            }
        }
    }

    public override void _Ready()
    {
        Stylus = Pen.pencil;
        color = penColor.black;
        line = lineThickness.one;
        drawnSprites = new bodyPart[6];
        createCanvas();
    }

    private void createCanvas()
    {
        backgroundSprite.Texture = backgroundTextures[textNameOrder[currentTextName]];
        GD.Print(DrawMessage[textNameOrder[currentTextName]]);
        label.Text = DrawMessage[textNameOrder[currentTextName]];
        canvas = new(size);
        pixels = new Area2D[size][];
        for (int i = 0; i < size; i++)
        {
            pixels[i] = new Area2D[size];
            for (int j = 0; j < size; j++)
            {
                int a = i;
                int b = j;
                pixels[i][j] = new Area2D();
                var collisionShape = new CollisionShape2D();
                var shape = new RectangleShape2D();
                shape.Size = new(pixelSize, pixelSize);
                collisionShape.Shape = shape;

                CallDeferred("add_child", pixels[i][j]);
                pixels[i][j].Position = new(i * pixelSize - (size * 0.5f * pixelSize), j * pixelSize - (size * 0.5f * pixelSize));
                pixels[i][j].CallDeferred("add_child", collisionShape);
                pixels[i][j].MouseEntered += () => MouseEntered(b, a);
            }
        }
        Texture = new();
    }
    private void MouseEntered(int i, int j)
    {
        if (drawing)
        {
            switch (Stylus)
            {
                case Pen.pencil:
                    DrawOnCanvas(i, j);
                    GD.Print($"Drawing at {i}, {j}");
                    break;
                case Pen.eraser:
                    canvas.Pixels[i][j].r = 255;
                    canvas.Pixels[i][j].g = 255;
                    canvas.Pixels[i][j].b = 255;
                    canvas.Pixels[i][j].a = 0;
                    var image = canvas.CreateImage();
                    var text = ImageTexture.CreateFromImage(image);
                    Texture = text;
                    break;
                default:
                    break;
            }
        }
    }

    public override void _Input(InputEvent e)
    {
        if (e is InputEventMouseMotion m)
        {

        }
        else if (e is InputEventMouseButton b)
        {
            if (b.IsActionPressed("PrimaryAction"))
            {
                drawing = true;
            }
            else if (b.IsActionReleased("PrimaryAction"))
            {
                drawing = false;
            }
        }
    }

    public void DrawOnCanvas(int i, int j)
    {
        Image img;
        ImageTexture texture;
        switch (color)
        {
            case penColor.black:
                setColors(i, j, 0, 0, 0, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 0, 0, 0, 255);
                }

                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.red:
                setColors(i, j, 255, 0, 0, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 255, 0, 0, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.green:
                setColors(i, j, 0, 150, 0, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 0, 150, 0, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.blue:
                setColors(i, j, 0, 0, 255, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 0, 0, 255, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.purple:
                setColors(i, j, 175, 0, 200, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 175, 0, 200, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.brown:
                setColors(i, j, 159, 75, 0, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 159, 75, 0, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.grey:
                setColors(i, j, 125, 125, 125, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 125, 125, 125, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.orange:
                setColors(i, j, 255, 140, 0, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 255, 140, 0, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.yellow:
                setColors(i, j, 255, 255, 0, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 255, 255, 0, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
            case penColor.pink:
                setColors(i, j, 255, 102, 179, 255);
                if (line == lineThickness.three || line == lineThickness.five)
                {
                    setSurroundingColors(i, j, 255, 102, 179, 255);
                }
                img = canvas.CreateImage();
                texture = ImageTexture.CreateFromImage(img);
                Texture = texture;
                break;
        }
    }

    public void setColors(int i, int j, byte r, byte g, byte b, byte a)
    {
        canvas.Pixels[i][j].r = r;
        canvas.Pixels[i][j].g = g;
        canvas.Pixels[i][j].b = b;
        canvas.Pixels[i][j].a = a;
    }

    public void setSurroundingColors(int i, int j, byte r, byte g, byte b, byte a)
    {
        var tempI = i;
        var tempJ = j;
        switch (line)
        {  
            case lineThickness.three:
                if(tempI - 1 >= 0)
                {
                    if(tempJ + 1 < size)
                    {
                        setColors(tempI - 1, tempJ + 1, r, g, b, a);
                    }
                    if(tempJ - 1 >= 0)
                    {
                        setColors(tempI - 1, tempJ - 1, r, g, b, a);
                    }
                    setColors(tempI - 1, tempJ, r, g, b, a);
                }
                if(tempI + 1 < size)
                {
                    if(tempJ + 1 < size)
                    {
                        setColors(tempI + 1, tempJ + 1, r, g, b, a);
                    }
                    if(tempJ - 1 >= 0)
                    {
                        setColors(tempI + 1, tempJ - 1, r, g, b, a);
                    }
                    setColors(tempI + 1, tempJ, r, g, b, a);
                }
                if(tempJ + 1 < size)
                {
                    setColors(tempI, tempJ + 1, r, g, b, a);
                }
                if(tempJ - 1 >= 0)
                {
                    setColors(tempI, tempJ - 1, r, g, b, a);
                }
                break;
        }
    }

    public void OnPencilClick()
    {
        Stylus = Pen.pencil;
    }

    public void OnEraserClick()
    {
        Stylus = Pen.eraser;
    }

    public void OnSubmitClick()
    {
        var img = canvas;
        textName tn = textNameOrder[currentTextName];
        sprites[tn] = img;
        currentTextName += 1;
        if(currentTextName >= textNameOrder.Count)
        {
            var n = stitchChar.Instantiate<StitchCharacter>();
            n.SetTextures(sprites);
            Globals.Instance.CreateCharacter(n);
        } else
        {
            createCanvas();
            
        }
    }

    public void OnBlackPress()
    {
        BlackButton.ToggleMode = true;
        GD.Print("Pressed Black");
        color = penColor.black;

    }

    public void OnRedPress()
    {
        GD.Print("Pressed Red");
        color = penColor.red;
    }

    public void OnGreenPress()
    {
        GD.Print("Pressed Green");
        color = penColor.green;
    }

    public void OnBluePress()
    {
        GD.Print("Pressed Blue");
        color = penColor.blue;
    }

    public void OnPurplePress()
    {
        GD.Print("Pressed Purple");
        color = penColor.purple;
    }

    public void OnBrownPress()
    {
        GD.Print("Pressed Brown");
        color = penColor.brown;
    }

    public void OnGreyPress()
    {
        GD.Print("Pressed Grey");
        color = penColor.grey;
    }

    public void OnOrangePress()
    {
        GD.Print("Pressed Orange");
        color = penColor.orange;
    }

    public void OnYellowPress()
    {
        GD.Print("Pressed Yellow");
        color = penColor.yellow;
    }

    public void OnPinkPress()
    {
        GD.Print("Pressed Pink");
        color = penColor.pink;
    }

    public void OnOnePress()
    {
        line = lineThickness.one;
    }

    public void OnThreePress()
    {
        line = lineThickness.three;
    }

    public void OnFivePress()
    {
        line = lineThickness.five;
    }
}