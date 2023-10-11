namespace Sparkle.Scene; 

public abstract class Scene : IDisposable {

    public readonly string name;
    
    private readonly Dictionary<int, Entity.Entity> _entities;

    private int _entityIds;
    
    public bool HasInitialized { get; private set; }
    public bool HasDisposed { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Scene class with the specified name.
    /// Also initializes an empty dictionary to hold entities within the scene.
    /// </summary>
    /// <param name="name">The name of the scene.</param>
    public Scene(string name) {
        this.name = name;
        _entities = new Dictionary<int, Entity.Entity>();
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
        foreach (Entity.Entity entity in _entities.Values) {
            entity.Update();
        }
    }
    
    /// <summary>
    /// Called after the Update method on each tick to further update dynamic elements and game logic.
    /// </summary>
    protected internal virtual void AfterUpdate() {
        foreach (Entity.Entity entity in _entities.Values) {
            entity.AfterUpdate();
        }
    }
    
    /// <summary>
    /// Is invoked at a fixed rate of every <see cref="Update"/> frames following the <see cref="GameSettings"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    protected internal virtual void FixedUpdate() {
        foreach (Entity.Entity entity in _entities.Values) {
            entity.FixedUpdate();
        }
    }
    
    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    protected internal virtual void Draw() {
        foreach (Entity.Entity entity in _entities.Values) {
            entity.Draw();
        }
    }
    
    /// <summary>
    /// Adds an entity to the collection and initializes it.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    public void AddEntity(Entity.Entity entity) {
        ThrowIfDisposed();
        entity.Id = _entityIds++;
        entity.Init();
        
        _entities.Add(entity.Id, entity);
    }
    
    /// <summary>
    /// Removes an entity from the collection and disposes of it.
    /// </summary>
    /// <param name="id">The ID of the entity to be removed.</param>
    public void RemoveEntity(int id) {
        ThrowIfDisposed();
        _entities[id].Dispose();
        _entities.Remove(id);
    }
    
    /// <summary>
    /// Removes an entity from the collection and disposes of it.
    /// </summary>
    /// <param name="entity">The entity to be removed.</param>
    public void RemoveEntity(Entity.Entity entity) {
        ThrowIfDisposed();
        RemoveEntity(entity.Id);
    }

    /// <summary>
    /// Retrieves an entity from the collection by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to be retrieved.</param>
    /// <returns>The entity associated with the specified ID.</returns>
    public Entity.Entity GetEntity(int id) {
        ThrowIfDisposed();
        return _entities[id];
    }

    /// <summary>
    /// Retrieves an array of all entities currently in the collection.
    /// </summary>
    /// <returns>An array containing all entities in the collection.</returns>
    public Entity.Entity[] GetEntities() {
        ThrowIfDisposed();
        return _entities.Values.ToArray();
    }
    
    /// <summary>
    /// Retrieves entities from the collection that have a specific tag.
    /// </summary>
    /// <param name="tag">The tag used to filter entities.</param>
    /// <returns>An enumerable of entities with the specified tag.</returns>
    public IEnumerable<Entity.Entity> GetEntitiesWithTag(string tag) {
        ThrowIfDisposed();
        
        foreach (Entity.Entity entity in _entities.Values) {
            if (entity.tag == tag) {
                yield return entity;
            }
        }
    }

    public void Dispose() {
        if (HasDisposed) return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        HasDisposed = true;
    }
    
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            foreach (Entity.Entity entity in _entities.Values) {
                entity.Dispose();
            }
            _entities.Clear();
            _entityIds = 0;
        }
    }
    
    protected void ThrowIfDisposed() {
        if (HasDisposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}