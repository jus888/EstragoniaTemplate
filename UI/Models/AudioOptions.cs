using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using System;
using System.Text.Json;

using static AudioManager;
using static Godot.DisplayServer;

namespace EstragoniaTemplate.UI.Models;

public partial class AudioOptions : ObservableObject
{
    private int _masterLevel = 100;
    public int MasterLevel
    {
        get => _masterLevel;
        set => _masterLevel = Mathf.Clamp(0, value, 100);
    }

    private int _musicLevel = 100;
    public int MusicLevel
    {
        get => _musicLevel;
        set => _musicLevel = Mathf.Clamp(0, value, 100);
    }

    private int _soundEffectsLevel = 100;
    public int SoundEffectsLevel
    {
        get => _soundEffectsLevel;
        set => _soundEffectsLevel = Mathf.Clamp(0, value, 100);
    }

    private int _interfaceLevel = 100;
    public int InterfaceLevel
    {
        get => _interfaceLevel;
        set => _interfaceLevel = Mathf.Clamp(0, value, 100);
    }

    public void Apply()
    {
        UpdateBusDbLevel(Bus.Master, MasterLevel);
        UpdateBusDbLevel(Bus.Music, MusicLevel);
        UpdateBusDbLevel(Bus.SFX, SoundEffectsLevel);
        UpdateBusDbLevel(Bus.UI, InterfaceLevel);
    }
}
