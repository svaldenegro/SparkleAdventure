using System.Numerics;
using GameContent;
using IdealAdventure.GameObjects;
using Raylib_cs;
using Sparkle.Entity;
using Sparkle.Scene;

namespace IdealAdventure.Scenes;

public class Main : Scene
{
    private Character _character = null!;
    private StaticModel _waitingRoom = null!;
    
    public Main(string name) : base(name)
    {
        var camera = SceneManager.MainCamera;
        if (camera is not null)
        {
            camera.position = new Vector3(0, 2, -5);
            camera.target = new Vector3(0, 1, 0);
        }
    }

    protected override void Init()
    {
        base.Init();
        
        Vector3 pos = new Vector3(0, 2f, 2.0f);
        Cam3D cam3D = new Cam3D(pos, 90, CameraMode.CAMERA_FREE) {
            target = new Vector3(0, 1f, 0),
            up = Vector3.UnitY
        };
        AddEntity(cam3D);
        
        Cam2D cam2D = new Cam2D(new Vector2(10, 10), new Vector2(10, 10), Cam2D.CameraMode.Normal);
        AddEntity(cam2D);
        
        _character = new Character(Vector3.UnitY);
        AddEntity(_character);

        _waitingRoom = new StaticModel(Vector3.Zero, ContentPath.WaitingRoomPath);
        AddEntity(_waitingRoom);
    }
}