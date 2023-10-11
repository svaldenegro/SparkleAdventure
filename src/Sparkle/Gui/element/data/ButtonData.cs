using Raylib_cs;

namespace Sparkle.Gui.element.data; 

public struct ButtonData {
    
    public Texture2D? texture;
    public float rotation;
    public Color color;
    public Color hoverColor;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonData"/>, setting default values for Rotation, Color, and HoverColor.
    /// </summary>
    public ButtonData() {
        rotation = 0;
        color = Color.WHITE;
        hoverColor = Color.GRAY;
    }
}