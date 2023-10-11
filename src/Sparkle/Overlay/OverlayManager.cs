namespace Sparkle.Overlay; 

public static class OverlayManager {
    
    public static readonly List<Overlay> Overlays = new();
    
    /// <summary>
    /// Used for Initializes objects.
    /// </summary>
    internal static void Init() {
        foreach (Overlay overlay in Overlays) {
            if (!overlay.HasInitialized) {
                overlay.Init();
            }
        }
    }
    
    /// <summary>
    /// Is invoked during each tick and is used for updating dynamic elements and game logic.
    /// </summary>
    internal static void Update() {
        foreach (Overlay overlay in Overlays) {
            if (overlay.Enabled) {
                overlay.Update();
            }
        }
    }
    
    /// <summary>
    /// Called after the Update method on each tick to further update dynamic elements and game logic.
    /// </summary>
    internal static void AfterUpdate() {
        foreach (Overlay overlay in Overlays) {
            if (overlay.Enabled) {
                overlay.AfterUpdate();
            }
        }
    }
    
    /// <summary>
    /// Is invoked at a fixed rate of every <see cref="Update"/> frames following the <see cref="GameSettings"/> method.
    /// It is used for handling physics and other fixed-time operations.
    /// </summary>
    internal static void FixedUpdate() {
        foreach (Overlay overlay in Overlays) {
            if (overlay.Enabled) {
                overlay.FixedUpdate();
            }
        }
    }
    
    /// <summary>
    /// Is called every tick, used for rendering stuff.
    /// </summary>
    internal static void Draw() {
        foreach (Overlay overlay in Overlays) {
            if (overlay.Enabled) {
                overlay.Draw();
            }
        }
    }
}