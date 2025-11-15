using System.Collections;
using System.Diagnostics;
using Godot;
using Steamworks;

public enum Pen
{
    pencil,
    eraser

};

public partial class DrawingCanvas : Sprite2D
{   
    [Export] private string spriteName;
    private SpriteArray2D canvas;
    private bool drawing = false;
    private Area2D[][] pixels;
    [Export] private float pixelSize = 1f;
    [Export] private int size = 32;
    private Pen stylus;

    public override void _Ready()
    {
        stylus = Pen.pencil;
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
    }
    private void MouseEntered(int i, int j)
    {
        GD.Print($"Drawing at {i},{j}");

        if (drawing)
        {
            switch (stylus) 
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

    public override void _Process(double delta)
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
                GD.Print("Drawing");
                drawing = true;
            } 
            else if (b.IsActionReleased("PrimaryAction"))
            {
                drawing = false;
            }
        }
    }

    public void onPencilClick()
    {
         
    }
}