namespace Sparkle.Overlay; 

public abstract class Overlay : IDisposable {
    
    public readonly string name;
    private bool _enabled;
    
    public bool HasInitialized { get; private set; }
    public bool HasDisposed { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Overlay"/>, setting its name and adding it to the OverlayManager's list of Overlays.
    /// </summary>
    /// <param name="name">The name of the Overlay instance.</param>
    public Overlay(string name) {
        this.name = name;
        OverlayManager.Overlays.Add(this);
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether the overlay is enabled.
    /// If set to true for the first time, initializes the overlay.
    /// Logs an error if the overlay is already disposed.
    /// </summary>
    public bool Enabled {
        get => _enabled;
        
        set {
            ThrowIfDisposed();
            _enabled = value;
        
            if (!HasInitialized) {
                Init();
                HasInitialized = true;
            }
        }
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
    protected internal virtual void Update() { }
    
    /// <summary>
    /// Called after the Update method on each tick to further update dynamic elements and game logic.
    /// </summary>
    protected internal virtual void AfterUpdate() { }

    /// <summary>
    /// Is invoked at a fixed rate of every <see cref="Update"/> frames following the <see cref="GameSettings"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    protected internal virtual void FixedUpdate() { }

    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    protected internal abstract void Draw();

    public void Dispose() {
        if (HasDisposed) return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        HasDisposed = true;
    }
    
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            _enabled = false;
            OverlayManager.Overlays.Remove(this);
        }
    }
    
    protected void ThrowIfDisposed() {
        if (HasDisposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}