using System.Numerics;
using Raylib_cs;
using Sparkle;
using Sparkle.Entity;
using Sparkle.Graphics.util;
using Sparkle.Gui;
using Sparkle.Scene;

namespace Test; 

public class TestScene : Scene {

    private TestGui _gui;

    public TestScene(string name) : base(name) {
        _gui = new TestGui("test");
    }

    protected override void Init() {

        Vector3 pos = new Vector3(0, 2f, -10.0f);
        Cam3D cam3D = new Cam3D(pos, 70, CameraMode.CAMERA_FREE) {
            target = new Vector3(0, 0, 0),
            up = Vector3.UnitY
        };
        AddEntity(cam3D);
        
        Cam2D cam2D = new Cam2D(new Vector2(10, 10), new Vector2(10, 10), Cam2D.CameraMode.Normal);
        AddEntity(cam2D);
        
        /*
        for (int i = 0; i < 1000; i++) {
            this.AddEntity(new TestEntity(new Vector3(0, i, 0)));
        }*/

        TestEntity entity = new TestEntity(new Vector3(0, 0, 0));
        AddEntity(entity);
        
        AddEntity(new GroundEntity(Vector3.Zero));
    }

    protected override void Update() {
        base.Update();
        
        if (Input.IsKeyPressed(KeyboardKey.KEY_E)) {
            GuiManager.SetGui(_gui);
        }

        if (Input.IsKeyPressed(KeyboardKey.KEY_R)) {
            GuiManager.SetGui(null);
        }
    }

    protected override void Draw() {
        base.Draw();
        
        // BEGIN 3D
        /*SceneManager.MainCamera!.BeginMode3D();

        // DRAW GIRD
        ModelHelper.DrawGrid(10, 1);
        
        // DRAW CUBE
        ModelHelper.DrawCube(new Vector3(3, 2, 3), 5, 5, 5, Color.PURPLE);
        
        // DRAW LINE
        ModelHelper.DrawLine3D(new Vector3(10, 3, 4), new Vector3(-10, -3, -4), Color.RED);
        
        // DRAW SECOND LINE
        ModelHelper.DrawLine3D(new Vector3(0, 3, 4), new Vector3(-10, -3, -4), Color.BLUE);
        
        ModelHelper.DrawCube(SceneManager.MainCamera.Target, 2, 2, 2, Color.RED);

        // END 3D
        SceneManager.MainCamera.EndMode3D();*/

        Cam2D cam2D = (Cam2D) GetEntity(1);

        if (Input.IsKeyDown(KeyboardKey.KEY_A)) {
            cam2D.target.X += 10.0F * Time.Delta;
        }
        
        cam2D.BeginMode2D();
        
        ShapeHelper.DrawRectangle((int) cam2D.target.X, (int) cam2D.target.Y, 5, 5, Color.WHITE);
        
        cam2D.EndMode2D();
    }
}