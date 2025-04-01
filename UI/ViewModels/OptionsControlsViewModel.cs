using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using Godot;
using System;
using EstragoniaTemplate.Main;
using System.Collections.ObjectModel;
using EstragoniaTemplate.UI.Models;
using System.Diagnostics;
using System.Collections.Generic;
using Avalonia.Data;
using EstragoniaTemplate.Resources;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsControlsViewModel : ViewModel, IOptionsTabViewModel
{
    [ObservableProperty]
    private ObservableCollection<InputMapItem> _navigationInputMapItems;
    [ObservableProperty]
    private ObservableCollection<InputMapItem> _gameplayInputMapItems;

    private readonly ViewModelFactory _viewModelFactory;
    private readonly MainViewModel _mainViewModel;

    private readonly UserInterface _currentUserInterface;
    private readonly UserInterface _dialogUserInterface;
    private readonly KeyRepeater _keyRepeater;

    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public OptionsControlsViewModel()
    {
        NavigationInputMapItems = new()
        {
            new("Confirm", Key.Enter, JoyButton.A, ["Keyboard/keyboard_a", "Keyboard/keyboard_b"]),
            new("Cancel", Key.Escape, JoyButton.X),
        };

        GameplayInputMapItems = new()
        {
            new("A", Key.A),
            new("B", Key.B)
        };
    }

    public OptionsControlsViewModel(ViewModelFactory viewModelFactory, MainViewModel mainViewModel, UserInterface currentUserInterface, UserInterface dialogUserInterface, KeyRepeater keyRepeater)
    {
        _viewModelFactory = viewModelFactory;
        _mainViewModel = mainViewModel;
        _currentUserInterface = currentUserInterface;
        _dialogUserInterface = dialogUserInterface;
        _keyRepeater = keyRepeater;

        SetInputMapItems();
    }

    private void SetInputMapItems()
    {
        HashSet<Key> navigationReservedKeys = new()
        {
            Key.Escape,
            Key.Enter,
            Key.Space
        };
        var navigationGroup = new InputMapGroup(navigationReservedKeys);
        NavigationInputMapItems = new()
        {
            new("ui_accept", "Confirm", navigationGroup, ["Keyboard/keyboard_enter", "Keyboard/keyboard_space"]),
            new("ui_cancel", "Cancel", navigationGroup),
            new("ui_left", "Left", navigationGroup, ["Keyboard/keyboard_arrow_left"]),
            new("ui_right", "Right", navigationGroup, ["Keyboard/keyboard_arrow_right"]),
            new("ui_up", "Up", navigationGroup, ["Keyboard/keyboard_arrow_up"]),
            new("ui_down", "Down", navigationGroup, ["Keyboard/keyboard_arrow_down"])
        };

        var gameplayGroup = new InputMapGroup();
        GameplayInputMapItems = new()
        {
            new("game_accept", "Confirm", gameplayGroup),
            new("game_cancel", "Cancel", gameplayGroup, ["Mouse/mouse_right"])
        };
    }

    [RelayCommand]
    public void ResetToDefault()
    {
        var dialog = new DialogViewModel(
            "Are you sure you want to reset all control bindings to their defaults?\n" +
            "Any made changes will be lost.",
            "Cancel", confirmText: "Reset to default"
            );
        dialog.Responded += OnResponse;

        void OnResponse(DialogViewModel.Response response)
        {
            dialog.Responded -= OnResponse;
            if (response == DialogViewModel.Response.Confirm)
            {
                InputMap.LoadFromProjectSettings();
                SetInputMapItems();
            }

            _keyRepeater.UpdateDirectionalKeys();
        }

        _mainViewModel.NavigateTo(dialog);
        if (_currentUserInterface != null && _dialogUserInterface != null)
        {
            _dialogUserInterface.StealFocus(_currentUserInterface, true);
        }
    }

    [RelayCommand]
    public void InputPromptKeyboard(InputMapItem inputMapItem)
        => InputPrompt(inputMapItem, true);

    [RelayCommand]
    public void InputPromptJoypad(InputMapItem inputMapItem)
        => InputPrompt(inputMapItem, false);

    public void InputPrompt(InputMapItem inputMapItem, bool listenToKeyboard)
    {
        var dialog = new InputListenerDialogViewModel(_dialogUserInterface, inputMapItem.GroupReservedKeys, listenToKeyboard, inputMapItem.InputName);
        dialog.InputPressed += OnInput;

        void OnInput((Key?, JoyButton?) inputTuple)
        {
            dialog.InputPressed -= OnInput;
            var (key, joyButton) = inputTuple;
            if (key != null)
            {
                inputMapItem.SetKeyboardKey(key.Value);
            }
            else if (joyButton != null)
            {
                inputMapItem.SetJoypadButton(joyButton.Value);
            }

            _keyRepeater.UpdateDirectionalKeys();
        }

        _mainViewModel.NavigateTo(dialog);
        if (_currentUserInterface != null && _dialogUserInterface != null)
        {
            _dialogUserInterface.StealFocus(_currentUserInterface, true);
        }
    }

    public void TryClose(Action callOnClose)
    {
        InputMapResource.SaveCurrentInputMap();
        callOnClose();
    }
}
