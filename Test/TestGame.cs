using Raylib_cs;
using Sparkle;
using Sparkle.File.config;
using Sparkle.Window;

namespace Test; 

public class TestGame : Game {

    public TestOverlay Overlay;

    public TestGame(GameSettings settings) : base(settings) { }

    protected override void Init() {
        base.Init();

        Overlay = new TestOverlay("Test");
        Overlay.Enabled = false;
        
        Config config = new ConfigBuilder("config", "test")
            .Add("test", true)
            .Add("lol", 1000)
            .Add("hello", "Hello World!")
            .Build();
        
        Console.WriteLine(config.GetValue<string>("hello"));
        Console.WriteLine(config.GetValue<int>("lol"));
    }

    protected override void Update() {
        base.Update();
        
        if (Input.IsKeyPressed(KeyboardKey.KEY_F11)) {
            Window.Maximize();
            Window.ToggleFullscreen();
        }
    }
}