using Raylib_cs;
using Sparkle;
using Test;

GameSettings settings = new GameSettings() {
    title = "Test - [Sparkle]",
    windowFlags = ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE
};

using TestGame game = new TestGame(settings);
game.Run(new TestScene("test"));