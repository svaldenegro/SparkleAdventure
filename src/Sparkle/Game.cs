using System.Reflection;
using Raylib_cs;
using Sparkle.Audio;
using Sparkle.Content;
using Sparkle.Graphics.util;
using Sparkle.Gui;
using Sparkle.Overlay;
using Sparkle.Physics;
using Sparkle.Scene;

namespace Sparkle; 

public class Game : IDisposable {
    
    public static Game Instance { get; private set; }
    public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version!;
    
    private readonly double _delay;
    private double _timer;
    
    public readonly GameSettings settings;
    public bool shouldClose;
    
    public ContentManager Content { get; private set; }
    public Simulation Simulation { get; private set; }
    
    public Image Logo { get; private set; }
    public Color BackgroundColor { get; set; } = Color.SKYBLUE;
    
    public bool HasInitialized { get; private set; }
    public bool HasDisposed { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/>, setting the static Instance to this object, initializing game settings, and calculating the delay based on the FixedTimeStep.
    /// </summary>
    /// <param name="settings">The game settings to be used for this Game instance.</param>
    public Game(GameSettings settings) {
        Instance = this;
        this.settings = settings;
        _delay = 1.0F / settings.fixedTimeStep;
    }
    
    /// <summary>
    /// Starts the <see cref="Game"/>.
    /// </summary>
    /// <param name="scene">The initial <see cref="Scene"/> to start with.</param>
    public void Run(Scene.Scene? scene) {
        ThrowIfDisposed();
        
        if (settings.logDirectory != string.Empty) {
            Logger.CreateLogFile(settings.logDirectory);
        }

        Logger.Info($"Hello World! Sparkle [{Version}] start...");
        Logger.Info($"\tCPU: {SystemInfo.Cpu}");
        Logger.Info($"\tMEMORY: {SystemInfo.MemorySize} GB");
        Logger.Info($"\tTHREADS: {SystemInfo.Threads}");
        Logger.Info($"\tOS: {SystemInfo.Os}");
        
        Logger.Debug("Initialize Raylib logger...");
        Logger.SetupRayLibLogger();
        
        Logger.Debug($"Setting target fps to: {(settings.targetFps > 0 ? settings.targetFps : "unlimited")}");
        SetTargetFps(settings.targetFps);

        Logger.Debug("Initialize content manager...");
        Content = new ContentManager(settings.contentDirectory);
        
        Logger.Debug("Initialize audio device...");
        AudioDevice.Init();

        Logger.Debug("Initialize window...");
        Window.Window.SetConfigFlags(settings.windowFlags);
        Window.Window.Init(settings.windowWidth, settings.windowHeight, settings.title);
            
        Logo = settings.iconPath == string.Empty ? ImageHelper.Load("Icon/Icon.png") : Content.Load<Image>(settings.iconPath);
        Window.Window.SetIcon(Logo);
        
        Logger.Debug("Initialize physics...");
        Simulation = new Simulation(settings.physicsSettings);
        
        Logger.Debug("Initialize default scene...");
        SceneManager.SetDefaultScene(scene!);
        
        Init();
        HasInitialized = true;
        
        Logger.Debug("Run ticks...");
        while (!shouldClose && !Window.Window.ShouldClose()) {
            Update();
            AfterUpdate();
            
            _timer += Time.Delta;
            while (_timer >= _delay) {
                FixedUpdate();
                _timer -= _delay;
            }
            
            Graphics.Graphics.BeginDrawing();
            Graphics.Graphics.ClearBackground(BackgroundColor);
            Draw();
            Graphics.Graphics.EndDrawing();
        }
        
        OnClose();
    }
    
    /// <summary>
    /// Used for Initializes objects.
    /// </summary>
    protected virtual void Init() {
        SceneManager.Init();
        OverlayManager.Init();
    }
    
    /// <summary>
    /// Is invoked during each tick and is used for updating dynamic elements and game logic.
    /// </summary>
    protected virtual void Update() {
        SceneManager.Update();
        GuiManager.Update();
        OverlayManager.Update();
    }

    /// <summary>
    /// Called after the Update method on each tick to further update dynamic elements and game logic.
    /// </summary>
    protected virtual void AfterUpdate() {
        SceneManager.AfterUpdate();
        GuiManager.AfterUpdate();
        OverlayManager.AfterUpdate();
    }

    /// <summary>
    /// Is invoked at a fixed rate of every <see cref="AfterUpdate"/> frames following the <see cref="GameSettings"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    protected virtual void FixedUpdate() {
        Simulation.Update(1.0F / settings.fixedTimeStep, 1);
        SceneManager.FixedUpdate();
        GuiManager.FixedUpdate();
        OverlayManager.FixedUpdate();
    }
    
    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    protected virtual void Draw() {
        SceneManager.Draw();
        GuiManager.Draw();
        OverlayManager.Draw();
    }
    
    /// <summary>
    /// Is called when the <see cref="Game"/> is shutting down.
    /// </summary>
    protected virtual void OnClose() {
        Logger.Warn("Application shuts down!");
    }

    /// <summary>
    /// Retrieves the frames per second (FPS) of the application.
    /// </summary>
    /// <returns>The current frames per second (FPS) value.</returns>
    public int GetFps() {
        ThrowIfDisposed();
        return Raylib.GetFPS();
    }

    /// <summary>
    /// Sets the target frames per second (FPS) for the application.
    /// </summary>
    /// <param name="fps">The desired target frames per second (FPS) value.</param>
    public void SetTargetFps(int fps) {
        ThrowIfDisposed();
        if (fps > 0) {
            Raylib.SetTargetFPS(fps);
        }
    }

    /// <inheritdoc cref="Raylib.OpenURL(string)"/>
    public void OpenUrl(string url) {
        ThrowIfDisposed();
        Raylib.OpenURL(url);
    }

    public void Dispose() {
        if (HasDisposed) return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        HasDisposed = true;
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            if (settings.iconPath == string.Empty) {
                ImageHelper.Unload(Logo);
            }

            for (int i = 0; i < OverlayManager.Overlays.Count; i++) {
                OverlayManager.Overlays[i].Dispose();
            }

            Content.Dispose();
            Window.Window.Close();
            AudioDevice.Close();
            GuiManager.ActiveGui?.Dispose();
            SceneManager.ActiveScene?.Dispose();
            Simulation.Dispose();
        }
    }
    
    protected void ThrowIfDisposed() {
        if (HasDisposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}