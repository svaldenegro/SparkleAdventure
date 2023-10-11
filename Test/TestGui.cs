using System.Numerics;
using Sparkle.Graphics.util;
using Sparkle.Gui;
using Sparkle.Gui.element;
using Sparkle.Gui.element.data;
using Sparkle.Window;
using Color = Raylib_cs.Color;

namespace Test; 

public class TestGui : Gui {
    
    public TestGui(string name) : base(name) { }

    protected override void Init() {
        base.Init();
/*
        LabelData labelData = new LabelData() {
            Font = FontHelper.GetDefault(),
            FontSize = 50,
            Spacing = 4,
            Text = "Sparkle Engine!",
            Color = Color.WHITE,
            HoverColor = Color.GRAY,
            Rotation = 0
        };
        
        this.AddElement(new LabelElement("label", labelData, new Vector2(0, 0)));

        ButtonData buttonData = new ButtonData() {
            Texture = this.Content.Load<Texture2D>("icon.png")
        };
        
        LabelData buttonLabelData = new LabelData() {
            Font = FontHelper.GetDefault(),
            FontSize = 25,
            Spacing = 4,
            Text = "Sparkle Engine!",
            Color = Color.RED
        };
        
        this.AddElement(new ButtonElement("button", buttonData, buttonLabelData, new Vector2(500, 400), new Vector2(300, 300), () => {
            Logger.Error("BUTTON GET PRESSED!");
            return true;
        }));
*/
        ToggleData toggleData = new ToggleData() {
            toggledText = "Checked"
        };
        
        LabelData toggleLabelData = new LabelData() {
            font = FontHelper.GetDefault(),
            fontSize = 25,
            spacing = 4,
            text = "Check",
            color = Color.RED
        };

        Vector2 pos = new Vector2((Window.GetRenderWidth() - 100F) / 2F, (Window.GetRenderHeight() - 100F) / 2F);
        AddElement(new ToggleElement("toggle", toggleData, toggleLabelData, pos, new Vector2(100, 100)));
    }

    protected override void Update() {
        base.Update();
        //Vector2 pos = new Vector2((this.Window.GetRenderWidth() - 100F) / 2F, (this.Window.GetRenderHeight() - 100F) / 2F);

        //this.GetElement("toggle").Position = pos;
    }
}