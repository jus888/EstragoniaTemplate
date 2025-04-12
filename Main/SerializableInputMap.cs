using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EstragoniaTemplate.Main;

public class InputMapKeyEvent
{
    public string InputActionName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Key PhysicalKey { get; set; }

    public InputMapKeyEvent() { }
    public InputMapKeyEvent(string action, Key physicalKey)
    {
        InputActionName = action;
        PhysicalKey = physicalKey;
    }
}

public class InputMapJoypadEvent
{
    public string InputActionName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public JoyButton JoypadButton { get; set; }

    public InputMapJoypadEvent() { }
    public InputMapJoypadEvent(string action, JoyButton joypadButton)
    {
        InputActionName = action;
        JoypadButton = joypadButton;
    }
}

public class SerializableInputMap
{
    public List<InputMapKeyEvent> KeyEvents { get; set; } = new();
    public List<InputMapJoypadEvent> JoypadEvents { get; set; } = new();

    private static JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    public static void SaveCurrentInputMap()
    {
        var inputMap = new SerializableInputMap();

        foreach (var action in InputMap.GetActions())
        {
            foreach (var inputEvent in InputMap.ActionGetEvents(action))
            {
                if (inputEvent is InputEventKey key && key.PhysicalKeycode != Key.None)
                {
                    inputMap.KeyEvents.Add(new(action, key.PhysicalKeycode));
                }
                else if (inputEvent is InputEventJoypadButton joypadButton)
                {
                    inputMap.JoypadEvents.Add(new(action, joypadButton.ButtonIndex));
                }
            }
        }

        using var file = FileAccess.Open("user://input_map.json", FileAccess.ModeFlags.Write);
        file.StoreString(JsonSerializer.Serialize(inputMap, _jsonOptions));
    }

    public static void LoadAndApplyInputMap()
    {
        if (!FileAccess.FileExists("user://input_map.json"))
            return;
        
        using var file = FileAccess.Open("user://input_map.json", FileAccess.ModeFlags.Read);
        var inputMap = JsonSerializer.Deserialize<SerializableInputMap>(file.GetAsText(), _jsonOptions);

        if (inputMap == null)
            return;

        foreach (var action in InputMap.GetActions())
        {
            InputMap.ActionEraseEvents(action);
        }

        foreach (var keyEvent in inputMap.KeyEvents)
        {
            var inputEventKey = new InputEventKey()
            {
                PhysicalKeycode = keyEvent.PhysicalKey
            };

            using StringName action = keyEvent.InputActionName;
            InputMap.ActionAddEvent(action, inputEventKey);
        }

        foreach (var joypadEvent in inputMap.JoypadEvents)
        {
            var inputEventJoypad = new InputEventJoypadButton()
            {
                ButtonIndex = joypadEvent.JoypadButton
            };

            using StringName action = joypadEvent.InputActionName;
            InputMap.ActionAddEvent(action, inputEventJoypad);
        }
    }
}
