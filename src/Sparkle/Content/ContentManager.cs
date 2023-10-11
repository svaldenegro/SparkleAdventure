using Raylib_cs;
using Sparkle.Content.processor;

namespace Sparkle.Content; 

public class ContentManager : IDisposable {

    private readonly string _contentDirectory;

    private readonly List<object> _content;
    private readonly Dictionary<Type, IContentProcessor> _processors;
    
    public bool HasDisposed { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentManager"/>, setting the content directory and initializing internal collections and processors.
    /// </summary>
    /// <param name="directory">The directory where content will be located.</param>
    public ContentManager(string directory) {
        _contentDirectory = $"{directory}/";
        _content = new List<object>();
        
        _processors = new Dictionary<Type, IContentProcessor>();
        AddProcessors(typeof(Font), new FontProcessor());
        AddProcessors(typeof(Image), new ImageProcessor());
        AddProcessors(typeof(Texture2D), new TextureProcessor());
        AddProcessors(typeof(Model), new ModelProcessor());
        AddProcessors(typeof(Sound), new SoundProcessor());
        AddProcessors(typeof(Wave), new WaveProcessor());
        AddProcessors(typeof(Music), new MusicProcessor());
    }
    
    /// <summary>
    /// Adds a content processor to the collection for a specified content type.
    /// </summary>
    /// <param name="type">The type of content for which the processor will be used.</param>
    /// <param name="processor">The content processor to add.</param>
    public void AddProcessors(Type type, IContentProcessor processor) {
        ThrowIfDisposed();
        _processors.Add(type, processor);
    }

    /// <summary>
    /// Tries to retrieve a content processor for the specified content type.
    /// </summary>
    /// <param name="type">The type of content for which the processor is sought.</param>
    /// <returns>
    /// The content processor associated with the specified content type,
    /// or null if a matching processor is not found.
    /// </returns>
    public IContentProcessor TryGetProcessor(Type type) {
        ThrowIfDisposed();
        if (!_processors.TryGetValue(type, out IContentProcessor? processor)) {
            Logger.Error($"Unable to locate ContentProcessor for type [{type}]!");
        }

        return processor!;
    }

    /// <summary>
    /// Loads a content item of the specified type from the given path.
    /// </summary>
    /// <typeparam name="T">The type of content item to load.</typeparam>
    /// <param name="path">The path to the content item.</param>
    /// <returns>The loaded content item.</returns>
    public T Load<T>(string path) {
        ThrowIfDisposed();
        T item = (T) TryGetProcessor(typeof(T)).Load(_contentDirectory + path);

        _content.Add(item!);
        return item;
    }

    public static Model LoadModel(string path)
    {
        return Raylib.LoadModel($"Models/{path}");
    }
    
    /// <summary>
    /// Unloads the specified content item.
    /// </summary>
    /// <typeparam name="T">The type of content item to unload.</typeparam>
    /// <param name="item">The content item to unload.</param>
    public void Unload<T>(T item) {
        ThrowIfDisposed();
        if (_content.Contains(item!)) {
            TryGetProcessor(typeof(T)).Unload(item!);
            _content.Remove(item!);
        }
        else {
            Logger.Warn($"Unable to unload content for the specified type {typeof(T)}!");
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
            foreach (object item in _content.ToList()) {
                TryGetProcessor(item.GetType()).Unload(item);
                _content.Remove(item);
            }
        }
    }
    
    public void ThrowIfDisposed() {
        if (HasDisposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}