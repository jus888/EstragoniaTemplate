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

    public event EventHandler? Applied;

    public UIOptions() { }
    public UIOptions(UIOptions options)
    {
        SetFromOptions(options);
    }

    public void SetFromOptions(UIOptions options)
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
        DisplayServer.WindowSetMode(WindowMode);
        DisplayServer.WindowSetVsyncMode(VSync ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);

        AvaloniaLoader.Instance.UIScalingOption = UIScale;

        Engine.MaxFps = 0;
        if (!VSync)
        {
            Engine.MaxFps = FPSLimit;
        }
    }

    public void SaveOverrideFile()
    {
        using var file = FileAccess.Open("user://settings_override.cfg", FileAccess.ModeFlags.Write)!;
        file.StoreLine($"display/window/size/mode = {(int)WindowMode}");
    }

    public override bool Equals(object? obj)
    {
        return obj is UIOptions options &&
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
