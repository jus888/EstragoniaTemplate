using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using System;

namespace EstragoniaTemplate.UI.Models;

public partial class UIOptions : ObservableObject
{
    [ObservableProperty]
    private DisplayServer.WindowMode _windowMode = DisplayServer.WindowMode.Fullscreen;

    [ObservableProperty]
    private bool _vSync = true;

    [ObservableProperty]
    private int _FPSLimit = 60;

    [ObservableProperty]
    private float _UIScale = 1;

    public void Apply()
    {
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
        DisplayServer.WindowSetMode(WindowMode);
        DisplayServer.WindowSetVsyncMode(VSync ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);

        AvaloniaLoader.Instance.UIScalingOption = UIScale;

        Engine.MaxFps = 0;
        if (!VSync)
        {
            Engine.MaxFps = FPSLimit;
        }
    }
}
