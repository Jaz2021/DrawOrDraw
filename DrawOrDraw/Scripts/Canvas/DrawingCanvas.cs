using System.Collections;
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

public struct bodyPart
{
    public Image texture;
    public textName spriteName;
}

public partial class DrawingCanvas : Sprite2D
{   
    private bodyPart[] drawnSprites;
    [Export] private Texture2D SelectedPencil;
    [Export] private Texture2D UnselectedPencil;
    [Export] private Texture2D SelectedEraser;
    [Export] private Texture2D UnselectedEraser;
    [Export] private Button PencilButton;
    [Export] private Button EraserButton;
    private SpriteArray2D canvas;
    private bool drawing = false;
    private Area2D[][] pixels;
    [Export] private float pixelSize = 1f;
    [Export] private int size = 48;
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
    private Pen _stylus;

    public override void _Ready()
    {
        Stylus = Pen.pencil;
        drawnSprites = new bodyPart[6];

        createCanvas();
    }

    private void createCanvas()
    {
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
                pixels[i][j].MouseExited += () => MouseLeft(b, a);
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
                    canvas.Pixels[i][j].r = 0;
                    canvas.Pixels[i][j].g = 0;
                    canvas.Pixels[i][j].b = 0;
                    canvas.Pixels[i][j].a = 255;
                    var img = canvas.CreateImage();
                    var texture = ImageTexture.CreateFromImage(img);
                    Texture = texture;
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
    private void MouseLeft(int i, int j)
    {

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
        var img = canvas.CreateImage();
        
        for (int i = 0; i < 6; i++)
        {
            if (drawnSprites[i].texture == null)
            {
                drawnSprites[i].texture = img;
                switch (i)
                {
                    case 0:
                        drawnSprites[i].spriteName = textName.head;
                        break;
                    case 1:
                        drawnSprites[i].spriteName = textName.torso;
                        break;
                    case 2:
                        drawnSprites[i].spriteName = textName.upper_arm;
                        break;
                    case 3:
                        drawnSprites[i].spriteName = textName.lower_arm;
                        break;
                    case 4:
                        drawnSprites[i].spriteName = textName.thigh;
                        break;
                    case 5:
                        drawnSprites[i].spriteName = textName.shin;
                        break;
                    default:
                        break;
                }
                createCanvas();
                return;
            }
        }

        // Go to stitching process;
        return;
    }
}