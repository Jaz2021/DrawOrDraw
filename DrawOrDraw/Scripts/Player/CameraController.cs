using Godot;


public partial class CameraController : Node2D
{
	private Camera2D cam;
	public override void _Ready()
	{
		cam = GetChild<Camera2D>(0);
	}
	public void Enable()
	{
		cam.Visible = true;
	}
	public void Disable()
	{
		cam.Visible = false;
	}
}
