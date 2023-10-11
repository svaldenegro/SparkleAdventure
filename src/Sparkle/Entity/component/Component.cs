namespace Sparkle.Entity.component; 

public abstract class Component : IDisposable {
    
    protected internal Entity Entity { get; internal set; }
    
    public bool HasInitialized { get; private set; }
    public bool HasDisposed { get; private set; }

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
    /// Is invoked at a fixed rate of every <see cref="GameSettings.fixedTimeStep"/> frames following the <see cref="Update"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    protected internal virtual void FixedUpdate() { }
    
    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    protected internal virtual void Draw() { }

    public void Dispose() {
        if (HasDisposed) return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        HasDisposed = true;
    }
    
    protected virtual void Dispose(bool disposing) { }
    
    public void ThrowIfDisposed() {
        if (HasDisposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}