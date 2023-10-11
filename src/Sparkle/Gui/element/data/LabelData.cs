using System.Numerics;
using Raylib_cs;
using Sparkle.Graphics.util;

namespace Sparkle.Gui.element.data;

public struct LabelData {
    
    public Font font;
    public string text;
    public float fontSize;
    public int spacing;
    public float rotation;
    public Vector2 size;
    public Color color;
    public Color hoverColor;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LabelData"/> with default settings. 
    /// Sets the default font, an empty text string, a font size of 18, a spacing of 4, 
    /// a rotation of 0, a size of (0,0), and default colors for normal and hover states.
    /// </summary>
    public LabelData() {
        font = FontHelper.GetDefault();
        text = string.Empty;
        fontSize = 18;
        spacing = 4;
        rotation = 0;
        size = Vector2.Zero;
        color = Color.WHITE;
        hoverColor = Color.GRAY;
    }
}