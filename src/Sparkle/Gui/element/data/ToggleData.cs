using Raylib_cs;

namespace Sparkle.Gui.element.data; 

public struct ToggleData {
    
    public Texture2D? texture;
    public Texture2D? toggledTexture;
    public float rotation;
    public Color color;
    public Color hoverColor;
    public Color toggledColor;

    public string toggledText;
    public Color toggledTextColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleData"/> with default settings.
    /// Sets the rotation to 0, assigns default colors for various states, and initializes an empty toggled text with a default color.
    /// </summary>
    public ToggleData() {
        rotation = 0;
        color = Color.WHITE;
        hoverColor = Color.GRAY;
        toggledColor = Color.WHITE;

        toggledText = string.Empty;
        toggledTextColor = Color.WHITE;
    }
}