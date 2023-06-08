﻿using Silk.NET.Maths;
using Silk.NET.Windowing;
using Sparkle.csharp;

ApplicationSettings settings = new ApplicationSettings() {
    Title = "Test",
    Size = new Vector2D<int>(1280, 720),
    WindowState = WindowState.Normal,
    WindowBorder = WindowBorder.Resizable,
    VSync = true,
    IsVisible = true,
    TargetFps = 0,
    Headless = false,
};

using Application application = new Application(settings);
application.Run();