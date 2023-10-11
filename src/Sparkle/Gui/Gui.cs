using Sparkle.Gui.element;

namespace Sparkle.Gui; 

public abstract class Gui : IDisposable {
    
    public readonly string name;

    private Dictionary<string, GuiElement> _elements;
    
    public bool HasInitialized { get; private set; }
    public bool HasDisposed { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Gui"/>, setting its name and initializing an empty dictionary to hold Gui elements.
    /// </summary>
    /// <param name="name">The name of the Gui instance.</param>
    public Gui(string name) {
        this.name = name;
        _elements = new Dictionary<string, GuiElement>();
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
        foreach (GuiElement element in _elements.Values) {
            element.Update();
        }
    }
    
    /// <summary>
    /// Called after the Update method on each tick to further update dynamic elements and game logic.
    /// </summary>
    protected internal virtual void AfterUpdate() {
        foreach (GuiElement element in _elements.Values) {
            element.AfterUpdate();
        }
    }

    /// <summary>
    /// Is invoked at a fixed rate of every <see cref="GameSettings.fixedTimeStep"/> frames following the <see cref="Update"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    protected internal virtual void FixedUpdate() {
        foreach (GuiElement element in _elements.Values) {
            element.FixedUpdate();
        }
    }

    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    protected internal virtual void Draw() {
        foreach (GuiElement element in _elements.Values) {
            if (element.enabled) {
                element.Draw();
            }
        }
    }
    
    /// <summary>
    /// Adds a GUI element to the collection and initializes it.
    /// </summary>
    /// <param name="element">The GUI element to be added.</param>
    protected void AddElement(GuiElement element) {
        ThrowIfDisposed();
        
        element.Init();
        _elements.Add(element.name, element);
    }
    
    /// <summary>
    /// Removes a GUI element from the collection.
    /// </summary>
    /// <param name="name">The name of the GUI element to be removed.</param>
    protected void RemoveElement(string name) {
        ThrowIfDisposed();
        _elements.Remove(name);
    }
    
    /// <summary>
    /// Removes a GUI element from the collection.
    /// </summary>
    /// <param name="element">The GUI element to be removed.</param>
    protected void RemoveElement(GuiElement element) {
        ThrowIfDisposed();
        RemoveElement(element.name);
    }

    /// <summary>
    /// Retrieves a GUI element from the collection by its name.
    /// </summary>
    /// <param name="name">The name of the GUI element to be retrieved.</param>
    /// <returns>The GUI element associated with the specified name.</returns>
    protected GuiElement GetElement(string name) {
        ThrowIfDisposed();
        return _elements[name];
    }

    /// <summary>
    /// Retrieves an array of all GUI elements currently in the collection.
    /// </summary>
    /// <returns>An array containing all GUI elements in the collection.</returns>
    protected GuiElement[] GetElements() {
        ThrowIfDisposed();
        return _elements.Values.ToArray();
    }

    public virtual void Dispose() {
        if (HasDisposed) return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        HasDisposed = true;
    }
    
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            foreach (GuiElement element in _elements.Values) {
                element.Dispose();
            }
            _elements.Clear();
        }
    }
    
    protected void ThrowIfDisposed() {
        if (HasDisposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}