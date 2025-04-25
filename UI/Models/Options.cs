using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Godot.DisplayServer;

namespace EstragoniaTemplate.UI.Models;

public class Options
{
    public GraphicsOptions GraphicsOptions { get; set; }
    public AudioOptions AudioOptions { get; set; }

    private static JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    public Options()
    {
        GraphicsOptions = new();
        AudioOptions = new();
    }

    public static Options LoadOrCreate()
    {
        Options options;
        if (FileAccess.FileExists("user://settings.json"))
        {
            using var readFile = FileAccess.Open("user://settings.json", FileAccess.ModeFlags.Read);
            try
            {
                options = JsonSerializer.Deserialize<Options>(readFile.GetAsText()) ?? new();
                options.Apply();
                return options;
            }
            catch (JsonException) { }
        }

        options = new();
        options.Apply();
        options.Save();

        return options;
    }

    public void Apply()
    {
        GraphicsOptions.Apply();
        AudioOptions.Apply();
    }

    public void Save()
    {
        using var file = FileAccess.Open("user://settings.json", FileAccess.ModeFlags.Write);
        file.StoreString(JsonSerializer.Serialize(this, _jsonOptions));

        using var overrideFile = FileAccess.Open("user://settings_override.cfg", FileAccess.ModeFlags.Write)!;
        overrideFile.StoreLine($"display/window/size/mode = {(int)GraphicsOptions.WindowMode}");
    }
}
