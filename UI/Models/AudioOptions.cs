using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using System;
using System.Text.Json;

using static AudioManager;
using static Godot.DisplayServer;

namespace EstragoniaTemplate.UI.Models;

public partial class AudioOptions : ObservableObject
{
    public int MasterLevel { get; set; }
    public int MusicLevel { get; set; }
    public int SoundEffectsLevel { get; set; }
    public int InterfaceLevel { get; set; }

    public static AudioOptions LoadOrCreateOptions()
    {
        AudioOptions options;
        if (FileAccess.FileExists("user://audioSettings.json"))
        {
            using var readFile = FileAccess.Open("user://audioSettings.json", FileAccess.ModeFlags.Read);
            try
            {
                options = JsonSerializer.Deserialize<AudioOptions>(readFile.GetAsText()) ?? new();
                options.Apply();
                return options;
            }
            catch (JsonException) { }
        }

        options = new AudioOptions();
        options.Save();
        options.Apply();

        return options;
    }

    public void Save()
    {
        using var file = FileAccess.Open("user://audioSettings.json", FileAccess.ModeFlags.Write);
        file.StoreString(JsonSerializer.Serialize(this));
    }

    public void Apply()
    {
        UpdateBusDbLevel(Bus.Master, MasterLevel);
        UpdateBusDbLevel(Bus.Music, MusicLevel);
        UpdateBusDbLevel(Bus.SFX, SoundEffectsLevel);
        UpdateBusDbLevel(Bus.UI, InterfaceLevel);
    }
}
