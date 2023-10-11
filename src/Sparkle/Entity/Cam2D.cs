using System.Numerics;
using Raylib_cs;

namespace Sparkle.Entity; 

public class Cam2D : Entity {
    
    private Camera2D _camera2D;
    
    public Vector2 target;
    public CameraMode mode;
    public float maxZoom;
    
    public Cam2D(Vector2 position, Vector2 target, CameraMode mode) : base(new Vector3(position.X, position.Y, 0)) {
        _camera2D = new Camera2D();
        this.target = target;
        this.mode = mode;
    }

    public new Vector2 Position {
        get => _camera2D.target;
        set => _camera2D.target = value;
    }
    
    public new float Rotation {
        get => _camera2D.rotation;
        set => _camera2D.rotation = value;
    }
    
    public Vector2 Offset {
        get => _camera2D.offset;
        set => _camera2D.offset = value;
    }
    
    public float Zoom {
        get => _camera2D.zoom;
        set {
            if (value < maxZoom) {
                _camera2D.zoom = value;
            }
        }
    }

    protected internal override void Update() {
        base.Update();
        Zoom += Input.GetMouseWheelMove() * 0.05F;

        switch (mode) {
            case CameraMode.Normal:
                NormalMovement(Window.Window.GetScreenWidth(), Window.Window.GetScreenHeight());
                break;
            
            case CameraMode.Smooth:
                SmoothMovement(Window.Window.GetScreenWidth(), Window.Window.GetScreenHeight());
                break;
        }
    }

    protected void NormalMovement(int width, int height) {
        Position = target;
        Offset = new Vector2(width / 2.0F, height / 2.0F);
    }
    
    protected void SmoothMovement(int width, int height) {
        float minSpeed = 30;
        float minEffectLength = 10;
        float fractionSpeed = 0.8f;

        Offset = new Vector2(width / 2.0F, height / 2.0F);
        Vector2 diff = target - Position;
        float length = diff.Length();

        if (length > minEffectLength) {
            float speed = Math.Max(fractionSpeed * length, minSpeed);
            Position = Vector2.Add(Position, Vector2.Multiply(diff, speed * Time.Delta / length));
        }
    }
    
    public enum CameraMode {
        Normal,
        Smooth,
        Custom
    }
    
    public void BeginMode2D() {
        Raylib.BeginMode2D(_camera2D);
    }

    public void EndMode2D() {
        Raylib.EndMode2D();
    }
}