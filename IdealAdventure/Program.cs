using IdealAdventure;
using IdealAdventure.Scenes;
using Raylib_cs;
using Sparkle;

GameSettings settings = new GameSettings() {
    title = "Test - [Sparkle]",
    windowFlags = ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE
};

AdventureGame game = new (settings);
game.Run(new Main("Main Scene"));