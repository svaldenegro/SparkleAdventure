using Raylib_cs;
using Sparkle;
using Sparkle.Window;

namespace IdealAdventure;

public class AdventureGame : Game
{
    public AdventureGame(GameSettings settings) : base(settings)
    {
    }

    protected override void Update()
    {
        if (!Input.IsKeyPressed(KeyboardKey.KEY_F11)) return;
        Window.Maximize();
        Window.ToggleFullscreen();
    }
}