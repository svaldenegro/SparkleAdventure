using System.Numerics;
using Raylib_cs;
using Sparkle.Graphics.util;
using Sparkle.Gui.element.data;

namespace Sparkle.Gui.element; 

public class LabelElement : GuiElement {
    
    public Font font;
    public float rotation;
    public Color color;
    public Color hoverColor;
    
    public string text;
    public float fontSize;
    public int spacing;
    
    protected float CalcFontSize { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LabelElement"/> with the given parameters, inheriting from a base class and setting various properties related to the label data.
    /// </summary>
    /// <param name="name">The name of the LabelElement.</param>
    /// <param name="data">Data for initializing label-specific properties like Font, Rotation, and Colors.</param>
    /// <param name="position">Position of the LabelElement on the screen.</param>
    /// <param name="clickClickFunc">Optional click function to be executed when the label is clicked.</param>
    public LabelElement(string name, LabelData data, Vector2 position, Func<bool>? clickClickFunc = null) : base(name, position, data.size, clickClickFunc) {
        font = data.font;
        rotation = data.rotation;
        color = data.color;
        hoverColor = data.hoverColor;
        
        text = data.text;
        fontSize = data.fontSize;
        spacing = data.spacing;
    }

    protected internal override void Update() {
        CalcFontSize = fontSize * GuiManager.Scale;
        size = FontHelper.MeasureText(font, text, CalcFontSize, spacing);
        base.Update();
    }

    protected internal override void Draw() {
        if (text != string.Empty) {
            Vector2 textPos = new Vector2(CalcPos.X + CalcSize.X / 2, CalcPos.Y + CalcSize.Y / 2);
            Vector2 textOrigin = new Vector2(CalcSize.X / 2, CalcSize.Y / 2);
            FontHelper.DrawText(font, text, textPos, textOrigin, rotation, CalcFontSize, spacing, isHovered ? hoverColor : color);
        }
    }
}