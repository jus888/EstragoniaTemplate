using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using System;
using System.Text.Json;

namespace EstragoniaTemplate.UI.Models;

public partial class GraphicsOptions : ObservableObject
{
    public int MaxFPSLimit => 300;
    public int MinFPSLimit => 60;

    [ObservableProperty]
    private DisplayServer.WindowMode _windowMode = DisplayServer.WindowMode.Fullscreen;

    [ObservableProperty]
    private bool _vSync = false;

    private int _fpsLimit = 60;
    public int FPSLimit
    {
        get => _fpsLimit;
        set => SetProperty(ref _fpsLimit, Mathf.Clamp(value, MinFPSLimit, MaxFPSLimit));
    }

    [ObservableProperty]
    private float _UIScale = 1;

    public event EventHandler? Applied;

    public GraphicsOptions() { }
    public GraphicsOptions(GraphicsOptions options)
    {
        SetFromOptions(options);
    }

    public GraphicsOptions SetFPSLimitToRefreshRate()
    {
        var refreshRate = DisplayServer.ScreenGetRefreshRate();
        if (refreshRate > 0)
        {
            FPSLimit = (int)refreshRate;
        }

        return this;
    }

    public void SetFromOptions(GraphicsOptions options)
    {
        WindowMode = options.WindowMode;
        VSync = options.VSync;
        FPSLimit = options.FPSLimit;
        UIScale = options.UIScale;
    }

    public void Apply()
    {
        Applied?.Invoke(this, EventArgs.Empty);

        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
        DisplayServer.WindowSetVsyncMode(VSync ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);

        var currentMode = DisplayServer.WindowGetMode();
        if (WindowMode != DisplayServer.WindowMode.Windowed || currentMode != DisplayServer.WindowMode.Maximized)
        {
            DisplayServer.WindowSetMode(WindowMode);
        }

        AvaloniaLoader.Instance.UIScalingOption = UIScale;

        Engine.MaxFps = 0;
        if (!VSync)
        {
            Engine.MaxFps = FPSLimit;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is GraphicsOptions options &&
               WindowMode == options.WindowMode &&
               VSync == options.VSync &&
               FPSLimit == options.FPSLimit &&
               UIScale == options.UIScale;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(WindowMode, VSync, FPSLimit, UIScale);
    }
}
