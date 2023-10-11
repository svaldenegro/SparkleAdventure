using System.Numerics;
using Raylib_cs;
using Sparkle.Graphics.util;
using Sparkle.Gui.element.data;

namespace Sparkle.Gui.element; 

public class ToggleElement : GuiElement {
    
    public Texture2D? texture;
    public Texture2D? toggledTexture;
    public float rotation;
    public Color color;
    public Color hoverColor;
    public Color toggledColor;
    
    public Font font;
    public float textRotation;
    public Vector2 textSize;
    public Color textColor;
    public Color textHoverColor;
    public Color toggledTextColor;
    
    public string text;
    public string toggledText;
    public float fontSize;
    public int spacing;
    
    protected float CalcFontSize { get; private set; }
    
    public bool IsToggled { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleElement"/> with the given parameters. Inherits from a base class and sets various properties related to toggle and label data.
    /// </summary>
    /// <param name="name">The name of the ToggleElement.</param>
    /// <param name="toggleData">Data for initializing toggle-specific properties like Textures, Rotation, and Colors.</param>
    /// <param name="labelData">Data for initializing label-specific properties like Font, Text, and Colors.</param>
    /// <param name="position">Position of the ToggleElement on the screen.</param>
    /// <param name="size">Optional size of the ToggleElement. Will default to the texture size if not provided and a texture exists.</param>
    /// <param name="clickClickFunc">Optional click function to be executed when the toggle is clicked.</param>
    public ToggleElement(string name, ToggleData toggleData, LabelData labelData, Vector2 position, Vector2? size, Func<bool>? clickClickFunc = null) : base(name, position, Vector2.Zero, clickClickFunc) {
        texture = toggleData.texture;
        toggledTexture = toggleData.toggledTexture;
        this.size = size ?? (texture != null ? new Vector2(texture.Value.width, texture.Value.height) : Vector2.Zero);
        rotation = toggleData.rotation;
        color = toggleData.color;
        hoverColor = toggleData.hoverColor;
        toggledColor = toggleData.toggledColor;
        
        font = labelData.font;
        textRotation = labelData.rotation;
        textColor = labelData.color;
        textHoverColor = labelData.hoverColor;
        toggledTextColor = toggleData.toggledTextColor;
        
        text = labelData.text;
        toggledText = toggleData.toggledText;
        fontSize = labelData.fontSize;
        spacing = labelData.spacing;
    }

    protected internal override void Update() {
        base.Update();
        GuiManager.SetScale(0.9F);
        
        if (isClicked) {
            IsToggled = !IsToggled;
        }
        
        CalcFontSize = fontSize * GuiManager.Scale;
        textSize = FontHelper.MeasureText(font, IsToggled ? toggledText : text, CalcFontSize, spacing);
    }
    
    protected internal override void Draw() {
        Texture2D? texture = IsToggled ? toggledTexture : this.texture;
        
        if (texture != null) {
            Rectangle source = new Rectangle(0, 0, texture.Value.width, texture.Value.height);
            Rectangle dest = new Rectangle(CalcPos.X + (CalcSize.X / 2), CalcPos.Y + (CalcSize.Y / 2), CalcSize.X, CalcSize.Y);
            Vector2 origin = new Vector2(dest.width / 2, dest.height / 2);
            Color color = isHovered ? hoverColor : (IsToggled ? toggledColor : this.color);
            TextureHelper.DrawPro(texture.Value, source, dest, origin, rotation, color);
        }
        else {
            Rectangle rec = new Rectangle(CalcPos.X + (CalcSize.X / 2), CalcPos.Y + (CalcSize.Y / 2), CalcSize.X, CalcSize.Y);
            Vector2 origin = new Vector2(rec.width / 2, rec.height / 2);
            Color color = isHovered ? hoverColor : (IsToggled ? toggledColor : this.color);
            ShapeHelper.DrawRectangle(rec, origin, rotation, color);
        }

        string text = IsToggled ? toggledText : this.text;
        if (text != string.Empty) {
            Vector2 textPos = new Vector2(CalcPos.X + CalcSize.X / 2, CalcPos.Y + CalcSize.Y / 2);
            Vector2 textOrigin = new Vector2(textSize.X / 2, textSize.Y / 2);
            Color textColor = isHovered ? textHoverColor : (IsToggled ? toggledTextColor : this.textColor);
            FontHelper.DrawText(font, text, textPos, textOrigin, textRotation, CalcFontSize, spacing, textColor);
        }
    }
}