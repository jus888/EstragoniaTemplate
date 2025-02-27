using EstragoniaTemplate.UI.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EstragoniaTemplate.Main;

public class KeyRepeater
{
    private const float SecondsUntilRepeat = 0.35f;
    private const float RepeatIntervalSeconds = 0.1f;

    readonly StringName[] _directionalInputEventNames = ["ui_left", "ui_right", "ui_up", "ui_down"];
    private InputEvent[] _directionalInputEvents = Array.Empty<InputEvent>();

    private Dictionary<InputEvent, float> _inputDownDurations = new(); 

    public KeyRepeater(UIOptions uiOptions)
    {
        uiOptions.Applied += (s, e) => UpdateDirectionalKeys();
        UpdateDirectionalKeys();
    }

    /// <summary>
    /// Returns false if the InputEvent should be handled further.
    /// </summary>
    public bool Input(InputEvent inputEvent)
    {
        var inputEventKey = inputEvent as InputEventKey;
        var joypadButton = inputEvent as InputEventJoypadButton;

        if (inputEventKey == null && joypadButton == null) return false;

        bool pressed = false;
        InputEvent? correspondingDirectionalEvent = null;
        foreach (var directionalEvent in _directionalInputEvents)
        {
            if (directionalEvent is InputEventKey keyEvent && keyEvent.PhysicalKeycode == inputEventKey?.PhysicalKeycode)
            {
                pressed = inputEventKey.Pressed;
                correspondingDirectionalEvent = directionalEvent;
                break;
            }

            if (directionalEvent is InputEventJoypadButton joypadEvent && joypadEvent.ButtonIndex == joypadButton?.ButtonIndex)
            {
                pressed = joypadButton.Pressed;
                correspondingDirectionalEvent = directionalEvent;
                break;
            }
        }

        if (correspondingDirectionalEvent == null)
            return false;

        if (_inputDownDurations.ContainsKey(correspondingDirectionalEvent))
        {
            if (!pressed)
            {
                _inputDownDurations.Remove(correspondingDirectionalEvent);
                return false;
            }
        }
        else if (pressed)
        {
            _inputDownDurations.Add(correspondingDirectionalEvent, 0);
            return false;
        }

        return true;
    }

    public void UpdateDirectionalKeys()
    {
        List<InputEvent> directionalEvents = new();
        foreach (var directionalName in _directionalInputEventNames)
        {
            var directionEvents = InputMap.ActionGetEvents(directionalName);
            directionalEvents.AddRange(directionEvents);
        }

        _directionalInputEvents = directionalEvents.ToArray();
        _inputDownDurations.Clear();
    }

    /// <summary>
    /// Calls _GuiInput on the userInterface for repeating keys.
    /// </summary>
    public void Process(float delta, UserInterface userInterface)
    {
        foreach (var (directionalInputEvent, duration) in _inputDownDurations)
        {
            var newDuration = duration + delta;

            if (newDuration > SecondsUntilRepeat - RepeatIntervalSeconds)
            {
                var remainder = newDuration - (SecondsUntilRepeat - RepeatIntervalSeconds);
                if (remainder > RepeatIntervalSeconds)
                {
                    userInterface.ForceGuiInput(CreatePressedInputEvent(directionalInputEvent));

                    newDuration -= RepeatIntervalSeconds;
                }
            }

            _inputDownDurations[directionalInputEvent] = newDuration;
        }
    }

    private InputEvent CreatePressedInputEvent(InputEvent inputEvent)
    {
        if (inputEvent is InputEventKey keyEvent)
        {
            return new InputEventKey()
            {
                Echo = true,
                Pressed = true,
                PhysicalKeycode = keyEvent.PhysicalKeycode,
            };
        }

        if (inputEvent is InputEventJoypadButton joypadEvent)
        {
            return new InputEventJoypadButton()
            {
                Pressed = true,
                ButtonIndex = joypadEvent.ButtonIndex,
                Device = joypadEvent.Device
            };
        }

        throw new ArgumentException("Argument was neither InputEventKey nor InputEventJoypadButton", nameof(inputEvent));
    }
}
