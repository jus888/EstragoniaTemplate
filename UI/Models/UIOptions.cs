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
}
