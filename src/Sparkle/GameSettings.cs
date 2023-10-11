using System.Reflection;
using Raylib_cs;
using Sparkle.Physics;

namespace Sparkle;

public struct GameSettings {
    
    public string title;
    public int windowWidth;
    public int windowHeight;
    public string iconPath;
    public string logDirectory;
    public string contentDirectory;
    public int targetFps;
    public int fixedTimeStep;
    public ConfigFlags windowFlags;
    public PhysicsSettings physicsSettings;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GameSettings"/> with default values for various game settings such as window size, icon path, log directory, content directory, and more.
    /// </summary>
    public GameSettings() {
        title = Assembly.GetEntryAssembly()!.GetName().Name ?? "Sparkle";
        windowWidth = 1280;
        windowHeight = 720;
        iconPath = string.Empty;
        logDirectory = "logs";
        contentDirectory = "content";
        targetFps = 0;
        fixedTimeStep = 60;
        windowFlags = ConfigFlags.FLAG_VSYNC_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE;
        physicsSettings = new PhysicsSettings();
    }
}