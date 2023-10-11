using System.Numerics;
using Raylib_cs;
using Sparkle.Gui;

namespace Sparkle.Entity; 

public class Cam3D : Entity {
    
    public Matrix4x4 View { get; private set; }
    public Matrix4x4 Projection { get; private set; }
    
    public CameraProjection projectionType;
    
    public float fov;
    public Vector3 up;

    public float aspectRatio;
    public float nearPlane;
    public float farPlane;

    public CameraMode mode;

    public float mouseSensitivity;
    public float gamepadSensitivity;

    public Vector3 target;

    private Vector3 _angleRot;
    public Vector3 AngleRot => _angleRot;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cam3D"/> with the specified position, field of view (fov), and camera mode.
    /// Also sets various camera properties such as tag, projection type, aspect ratio, near/far planes, and sensitivity.
    /// Finally, it initializes the View and Projection matrices for the camera.
    /// </summary>
    /// <param name="position">Initial position of the camera in 3D space, as a Vector3.</param>
    /// <param name="fov">Field of view angle, in degrees, in the y-axis.</param>
    /// <param name="mode">Camera movement mode, default is CameraMode.CAMERA_FREE.</param>
    public Cam3D(Vector3 position, float fov, CameraMode mode = CameraMode.CAMERA_FREE) : base(position) {
        tag = "camera";
        projectionType = CameraProjection.CAMERA_PERSPECTIVE;
        this.fov = fov;
        up = Vector3.UnitY;
        aspectRatio = (float) Window.Window.GetScreenWidth() / (float) Window.Window.GetScreenHeight();
        nearPlane = 0.01F;
        farPlane = 1000;
        target = position + Vector3.UnitZ;
        this.mode = mode;
        mouseSensitivity = 0.1F;
        gamepadSensitivity = 0.05F;
        
        View = Raymath.MatrixLookAt(this.position, target, up);
        Projection = GenProjection();
    }
    
    protected internal override void Update() {
        base.Update();

        CalculateTargetPosition();
        
        switch (mode) {
            case CameraMode.CAMERA_FREE:
                if (GuiManager.ActiveGui == null) {
                    InputController();
                }
                break;
            
            case CameraMode.CAMERA_ORBITAL:
                Matrix4x4 rotation = Raymath.MatrixRotate(up, -1.5F * Time.Delta);
                Vector3 view = position - target;
                Vector3 pos = Vector3.Transform(view, rotation);
                position = target + pos;
                
                if (GuiManager.ActiveGui == null) {
                    MoveToTarget(Input.GetMouseWheelMove());
                }
                break;
            
            case CameraMode.CAMERA_FIRST_PERSON:
                //TODO DO A OWN FIRST PERSON CAMERA
                break;
            
            case CameraMode.CAMERA_THIRD_PERSON:
                //TODO DO A OWN THIRD PERSON CAMERA
                break;
        }
    }
    
    /// <summary>
    /// Controls the entity's movement and rotation based on input from mouse, keyboard, or gamepad.
    /// </summary>
    private void InputController() {
        if (!Input.IsGamepadAvailable(0)) {
            float yaw = _angleRot.Y - (Input.GetMouseDelta().X * mouseSensitivity);
            float pitch = _angleRot.X + (Input.GetMouseDelta().Y * mouseSensitivity);
            RotateWithAngle(yaw, pitch, 0);

            if (Input.IsKeyDown(KeyboardKey.KEY_W)) {
                Move(new Vector3(0, 0, 1));
            }

            if (Input.IsKeyDown(KeyboardKey.KEY_S)) {
                Move(new Vector3(0, 0, -1));
            }

            if (Input.IsKeyDown(KeyboardKey.KEY_A)) {
                Move(new Vector3(1, 0, 0));
            }
                
            if (Input.IsKeyDown(KeyboardKey.KEY_D)) {
                Move(new Vector3(-1, 0, 0));
            }
            
            if (Input.IsKeyDown(KeyboardKey.KEY_SPACE)) {
                Move(new Vector3(0, 1, 0));
            }
            
            if (Input.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT)) {
                Move(new Vector3(0, -1, 0));
            }
        }
        else {
            float yaw = _angleRot.Y - (Input.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_RIGHT_X) * 2) * gamepadSensitivity;
            float pitch = _angleRot.X + (Input.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_RIGHT_Y) * 2) * gamepadSensitivity;
            RotateWithAngle(yaw, pitch, 0);
            
            if (Input.IsGamepadButtonDown(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_TRIGGER_2)) {
                MoveForward(1);
            }
            
            if (Input.IsGamepadButtonDown(0, GamepadButton.GAMEPAD_BUTTON_LEFT_TRIGGER_2)) {
                MoveForward(-1);
            }
        }
    }

    /// <summary>
    /// Calculates the target position based on the camera's mode and rotation.
    /// </summary>
    private void CalculateTargetPosition() {
        if (mode != CameraMode.CAMERA_ORBITAL) {
            Vector3 viewDir = Vector3.Transform(Vector3.UnitZ, rotation);
            target = position + viewDir;
        }
    }
    
    /// <summary>
    /// Retrieves the normalized forward direction vector of the camera.
    /// </summary>
    /// <returns>The normalized forward direction vector.</returns>
    public Vector3 GetForward() {
        return Vector3.Normalize(Vector3.Subtract(position, target));
    }
    
    /// <summary>
    /// Moves the camera forward or backward by a specified speed.
    /// </summary>
    /// <param name="speed">The speed of movement.</param>
    public void MoveForward(float speed) {
        position -= GetForward() * (speed * Time.Delta);
    }
    
    /// <summary>
    /// Moves the object towards a target position based on the given delta value.
    /// </summary>
    /// <param name="delta">The amount of movement to apply.</param>
    public void MoveToTarget(float delta) {
        float distance = Vector3.Distance(position, target);
        
        if (distance - delta <= 0) {
            return;
        }

        position += GetForward() * -delta;
    }
    
    /// <summary>
    /// Moves the object based on the provided speed vector.
    /// </summary>
    /// <param name="speedVector">The vector representing the movement speed along different axes.</param>
    public void Move(Vector3 speedVector) {
        Vector3 right = Vector3.Normalize(Vector3.Cross(up, GetForward()));
        
        position -= right * (speedVector.X * Time.Delta);
        position += up * (speedVector * Time.Delta);
        position -= GetForward() * (speedVector.Z * Time.Delta);
    }

    /// <summary>
    /// Rotates the object using the specified yaw, pitch, and roll angles.
    /// </summary>
    /// <param name="yaw">The yaw angle (rotation around the vertical axis) in degrees.</param>
    /// <param name="pitch">The pitch angle (rotation around the lateral axis) in degrees, clamped between -89 and 89 degrees.</param>
    /// <param name="roll">The roll angle (rotation around the longitudinal axis) in degrees.</param>
    public void RotateWithAngle(float yaw, float pitch, float roll) {
        _angleRot.Y = yaw % 360;
        _angleRot.X = Math.Clamp(pitch, -89, 89);
        _angleRot.Z = roll % 360;
        
        rotation = Quaternion.CreateFromYawPitchRoll(_angleRot.Y * Raylib.DEG2RAD, _angleRot.X * Raylib.DEG2RAD, _angleRot.Z * Raylib.DEG2RAD);
    }
    
    /// <summary>
    /// Generates a projection matrix based on the camera's projection type.
    /// </summary>
    /// <returns>The generated projection matrix.</returns>
    private Matrix4x4 GenProjection() {
        if (projectionType == CameraProjection.CAMERA_PERSPECTIVE) {
            return Raymath.MatrixPerspective(fov * Raylib.DEG2RAD, aspectRatio, nearPlane, farPlane);
        }
        else {
            float top = fov / 2.0F;
            float right = top * aspectRatio;
            
            return Raymath.MatrixOrtho(-right, right, -top, top, nearPlane, farPlane);
        }
    }
    
    /// <summary>
    /// Prepares the rendering context for 3D graphics by configuring matrices, projection, and depth testing.
    /// </summary>
    public void BeginMode3D() {
        Rlgl.rlDrawRenderBatchActive();
        Rlgl.rlMatrixMode(MatrixMode.PROJECTION);
        Rlgl.rlPushMatrix();
        Rlgl.rlLoadIdentity();
        
        aspectRatio = (float) Window.Window.GetScreenWidth() / (float) Window.Window.GetScreenHeight();

        Projection = GenProjection();
        Rlgl.rlSetMatrixProjection(Projection);
        
        Rlgl.rlMatrixMode(MatrixMode.MODELVIEW);
        Rlgl.rlLoadIdentity();
        
        View = Raymath.MatrixLookAt(position, target, up);
        Rlgl.rlMultMatrixf(View);
        
        Rlgl.rlEnableDepthTest();
    }
    
    /// <summary>
    /// Ends the 3D rendering mode and performs necessary cleanup.
    /// </summary>
    public void EndMode3D() {
        Raylib.EndMode3D();
    }
}