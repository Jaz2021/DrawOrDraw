using Godot;
using Networking_V2;
using Steamworks;
using System;
using System.Collections.Generic;

public partial class Globals : Node
{
	public static Globals Instance {
		get;
		private set;
	}
	[Export] public Node SceneRoot;
	[Export] public CanvasLayer MenuRoot;
	[Export] public CameraController PlayerCam;
	[Export] public Node EnvironmentRoot;
	[Export] private PackedScene InitialMenu;
	
	[Export] private PackedScene GameplayScene;
	[Export] private PackedScene PauseMenu;
	[Export] private PackedScene StitchChar;
	[Export] private PackedScene canvasScene;
	[Export] private PackedScene stageScene;
	private Scene currentScene = null;
	private MenuController currentMenu = null;
	private Env currentEnv = null;
	private bool inGame = false;
	private bool otherPlayerReady = false;
	private bool imReady = false;
	private StitchCharacter otherPlayerChar;
	// No initial scene, that will get loaded once the player starts the game
	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GD.PrintErr("Error, tried to recreate globals. This script should only exist on the outermost node in the tree");
			QueueFree();
		}
		ChangeMenu(InitialMenu);
		NetworkingV2.Init(false);
		StartGamePacket.StartGamePacketReceived += StartOnlineGame;
		SpritePacket.SpritePacketReceived += SpritePacketReceived;
	}

    private void SpritePacketReceived(SpritePacket packet, ConnectionManager connection)
    {
		GD.Print("I have received a sprite packet!");
        otherPlayerChar = StitchChar.Instantiate<StitchCharacter>();
		otherPlayerChar.bodyParts[textName.head] = packet.headSprite;
		otherPlayerChar.bodyParts[textName.torso] = packet.torsoSprite;
		otherPlayerChar.bodyParts[textName.shin] = packet.shinSprite;
		otherPlayerChar.bodyParts[textName.thigh] = packet.thighSprite;
		otherPlayerChar.bodyParts[textName.upper_arm] = packet.upperArmSprite;
		otherPlayerChar.bodyParts[textName.lower_arm] = packet.forearmSprite;
		otherPlayerReady = true;
        if (imReady)
        {
            ChangeScene(stageScene, Vector2.Zero);
        }
    }

    public void SendStartGamePacket(ConnectionManager connection)
	{
		List<ObjectStruct> objStructs = new();
		foreach(var obj in currentScene.objects)
        {
			objStructs.Add(new(obj.id, obj.Position, obj.type));
        }
		ObjectStructList objs = new(objStructs.ToArray());
		StartGamePacket packet = new(objs, Vector2.Zero); // Temporarily setting 0,0 as the start position
		GD.Print("Sending start game packet");
		NetworkingV2.SendPacket(connection, packet, true);
		// SpawnObjectPacket objPacket = new((ulong)connection.steamID, (byte)ObjectType.Player, Vector2.Zero);
		// NetworkingV2.SendPacketToAll(objPacket, true);
		// SpawnObjectPacket.SpawnObjectPacketReceived(objPacket, null);
		ChangeScene(canvasScene, Vector2.Zero, null);
		// ChangeMenu(PauseMenu);
		ChangeMenu(null);
	}
	public void StartOnlineGame()
    {
        NetworkingV2.Init(true);
		if (inGame)
		{
			return; // Ignore an extra start game packet
		}
		if (NetworkingV2.isInit)
		{
			
		}
		if ((ulong)NetworkingV2.GetLobbyID() == 0)
		{
			// We got startgame from ourself and there is no lobby yet.
			NetworkingV2.CreateLobby();
			ChangeScene(GameplayScene, Vector2.Zero);
			ChangeMenu(PauseMenu);
			return;
		}
		// We got the startgame from either ourselves or the lobby owner

		// SendStartGamePacket(
		return;
		
    }
	public void StartOnlineGame(StartGamePacket packet, ConnectionManager cnxn)
	{
		GD.Print("Got startgame packet");
		ChangeScene(canvasScene, Vector2.Zero);
		ChangeMenu(null);
	}
	public void StartOfflineGame()
    {
		ChangeScene(GameplayScene, Vector2.Zero);
		ChangeMenu(PauseMenu);
	}
	public override void _ExitTree()
	{
		GD.PrintErr("Warning, Globals was freed, something has probably gone wrong");
		Instance = null;
	}
	public void ChangeScene(PackedScene newScene, Vector2 position, ObjectStructList objs = null)
	{
		// All scenes must have a parent node that implements IScene.
		if(currentScene == null)

		{
            GD.Print("Current scene is null for some reason");
        }
		currentScene?.QueueFree();
        
		if(newScene != null)
		{
			GD.Print("New scene was not null");

			currentScene = newScene.Instantiate<Scene>();
			SceneRoot.AddChild(currentScene);
			currentScene.EnterScene(objs, position);
			// PlayerCam.Visible = true;
		} else
		{
			GD.Print("New scene was null");
			// PlayerCam.Visible = false;
		}
	}
	public void ChangeMenu(PackedScene newMenu)
	{
		// All menus must have a parent node that implements MenuController
		
		currentMenu?.QueueFree();
		if(newMenu == null)
        {
            return;
        }
		currentMenu = newMenu.Instantiate<MenuController>();
		MenuRoot.AddChild(currentMenu);
		currentMenu.EnterMenu();
	}
	public void ChangeEnvironment(PackedScene newEnvironment)
	{
		// All environments must have a parent node that implements IEnv
		currentEnv?.QueueFree();
		currentEnv = newEnvironment.Instantiate<Env>();
		currentEnv.Enter();
		EnvironmentRoot.AddChild(currentEnv);
	}
	public void CreateCharacter(StitchCharacter c)
    {
		GD.Print("Creating characters");
        SpritePacket packet = new(c.bodyParts[textName.lower_arm], c.bodyParts[textName.head], c.bodyParts[textName.shin], c.bodyParts[textName.thigh], c.bodyParts[textName.torso], c.bodyParts[textName.upper_arm]);
		NetworkingV2.SendPacketToAll(packet, true);
        if (otherPlayerReady)
        {
            ChangeScene(stageScene, Vector2.Zero);
			if(currentScene is StageScene s)
            {
                s.SpawnStitchedChars(c, otherPlayerChar);
            }
        }
		imReady = true;
		
    }
    public override void _Process(double delta)
    {
        if (NetworkingV2.isInit)
        {
			SteamAPI.RunCallbacks();
		}
	}
}
