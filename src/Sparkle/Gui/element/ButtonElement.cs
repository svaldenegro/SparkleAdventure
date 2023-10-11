using System.Numerics;
using Raylib_cs;
using Sparkle.Graphics.util;
using Sparkle.Gui.element.data;

namespace Sparkle.Gui.element; 

public class ButtonElement : GuiElement {
    
    public Texture2D? texture;
    public float rotation;
    public Color color;
    public Color hoverColor;
    
    public Font font;
    public float textRotation;
    public Vector2 textSize;
    public Color textColor;
    public Color textHoverColor;
    
    public string text;
    public float fontSize;
    public int spacing;
    
    protected float CalcFontSize { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonElement"/> with the given parameters. Inherits from a base class and sets various properties related to button and label data.
    /// </summary>
    /// <param name="name">The name of the ButtonElement.</param>
    /// <param name="buttonData">Data for initializing button-specific properties like Texture, Rotation, and Colors.</param>
    /// <param name="labelData">Data for initializing label-specific properties like Font, Text, and Colors.</param>
    /// <param name="position">Position of the ButtonElement on the screen.</param>
    /// <param name="size">Optional size of the ButtonElement. Will default to the texture size if not provided and a texture exists.</param>
    /// <param name="clickClickFunc">Optional click function to be executed when the button is clicked.</param>
    public ButtonElement(string name, ButtonData buttonData, LabelData labelData, Vector2 position, Vector2? size, Func<bool>? clickClickFunc = null) : base(name, position, Vector2.Zero, clickClickFunc) {
        texture = buttonData.texture;
        this.size = size ?? (texture != null ? new Vector2(texture.Value.width, texture.Value.height) : Vector2.Zero);
        rotation = buttonData.rotation;
        color = buttonData.color;
        hoverColor = buttonData.hoverColor;
        
        font = labelData.font;
        textRotation = labelData.rotation;
        textColor = labelData.color;
        textHoverColor = labelData.hoverColor;
        
        text = labelData.text;
        fontSize = labelData.fontSize;
        spacing = labelData.spacing;
    }

    protected internal override void Update() {
        base.Update();
        
        CalcFontSize = fontSize * GuiManager.Scale;
        textSize = FontHelper.MeasureText(font, text, CalcFontSize, spacing);
    }

    protected internal override void Draw() {
        if (texture != null) {
            Rectangle source = new Rectangle(0, 0, texture.Value.width, texture.Value.height);
            Rectangle dest = new Rectangle(CalcPos.X + (CalcSize.X / 2), CalcPos.Y + (CalcSize.Y / 2), CalcSize.X, CalcSize.Y);
            Vector2 origin = new Vector2(dest.width / 2, dest.height / 2);
            TextureHelper.DrawPro(texture.Value, source, dest, origin, rotation, isHovered ? hoverColor : color);
        }
        else {
            Rectangle rec = new Rectangle(CalcPos.X + (CalcSize.X / 2), CalcPos.Y + (CalcSize.Y / 2), CalcSize.X, CalcSize.Y);
            Vector2 origin = new Vector2(rec.width / 2, rec.height / 2);
            ShapeHelper.DrawRectangle(rec, origin, rotation, isHovered ? hoverColor : color);
        }

        if (text != string.Empty) {
            Vector2 textPos = new Vector2(CalcPos.X + CalcSize.X / 2, CalcPos.Y + CalcSize.Y / 2);
            Vector2 textOrigin = new Vector2(textSize.X / 2, textSize.Y / 2);
            FontHelper.DrawText(font, text, textPos, textOrigin, textRotation, CalcFontSize, spacing, isHovered ? textHoverColor : textColor);
        }
    }
}