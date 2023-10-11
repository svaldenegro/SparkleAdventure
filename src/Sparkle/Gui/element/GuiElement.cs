using System.Numerics;
using Raylib_cs;
using Sparkle.Graphics.util;
using Rectangle = Raylib_cs.Rectangle;

namespace Sparkle.Gui.element; 

public abstract class GuiElement : IDisposable {

    public readonly string name;
    public bool enabled;

    public Vector2 position;
    public Vector2 size;

    protected bool isHovered;
    protected bool isClicked;
    
    protected float WidthScale { get; private set; }
    protected float HeightScale { get; private set; }
    
    protected Vector2 CalcPos { get; private set; }
    protected Vector2 CalcSize { get; private set; }

    private Func<bool>? _clickFunc;
    
    public bool HasInitialized { get; private set; }
    public bool HasDisposed { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GuiElement"/> with specified parameters, setting its name, enabled state, position, size, and optional click function.
    /// </summary>
    /// <param name="name">The name of the GuiElement.</param>
    /// <param name="position">The position of the GuiElement on the screen.</param>
    /// <param name="size">The size of the GuiElement.</param>
    /// <param name="clickClickFunc">Optional click function to be executed when the GuiElement is clicked. Defaults to null.</param>
    public GuiElement(string name, Vector2 position, Vector2 size, Func<bool>? clickClickFunc) {
        this.name = name;
        enabled = true;
        this.position = position;
        this.size = size;
        _clickFunc = clickClickFunc!;
    }

    /// <summary>
    /// Used for Initializes objects.
    /// </summary>
    protected internal virtual void Init() {
        HasInitialized = true;
    }

    /// <summary>
    /// Is invoked during each tick and is used for updating dynamic elements and game logic.
    /// </summary>
    protected internal virtual void Update() {
        UpdateScale();
        
        CalcSize = new Vector2(size.X * GuiManager.Scale, size.Y * GuiManager.Scale);
        
        float differenceX = Math.Abs(size.X - CalcSize.X);
        float differenceY = Math.Abs(size.Y - CalcSize.Y);
        
        CalcPos = new Vector2((position.X + differenceX + CalcSize.X) * WidthScale - CalcSize.X, (position.Y + differenceY + CalcSize.Y) * HeightScale - CalcSize.Y);
        
        Rectangle rec = new Rectangle(CalcPos.X, CalcPos.Y, CalcSize.X, CalcSize.Y);
        if (ShapeHelper.CheckCollisionPointRec(Input.GetMousePosition(), rec)) {
            isHovered = true;

            if (Input.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && enabled) {
                isClicked = _clickFunc == null || _clickFunc.Invoke();
            }
            else {
                isClicked = false;
            }
        }
        else {
            isHovered = false;
            isClicked = false;
        }
    }
    
    /// <summary>
    /// Called after the Update method on each tick to further update dynamic elements and game logic.
    /// </summary>
    protected internal virtual void AfterUpdate() { }
    
    /// <summary>
    /// Is invoked at a fixed rate of every <see cref="GameSettings.fixedTimeStep"/> frames following the <see cref="Update"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    protected internal virtual void FixedUpdate() { }

    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    protected internal abstract void Draw();
    
    /// <summary>
    /// Updates the scaling factors for width and height based on the render dimensions.
    /// </summary>
    private void UpdateScale() {
        WidthScale = Window.Window.GetRenderWidth() / (float) Game.Instance.settings.windowWidth;
        HeightScale = Window.Window.GetRenderHeight() / (float) Game.Instance.settings.windowHeight;
    }

    public virtual void Dispose() {
        if (HasDisposed) return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        HasDisposed = true;
    }
    
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            enabled = false;
        }
    }
    
    public void ThrowIfDisposed() {
        if (HasDisposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}