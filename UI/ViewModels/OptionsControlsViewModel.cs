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

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsControlsViewModel : ViewModel, IOptionsTabViewModel
{
    public ObservableCollection<InputMapItem> InputMapItems { get; set; }

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
        InputMapItems = new()
        {
            new("", "Confirm", Key.Enter, JoyButton.A),
            new("", "Cancel", Key.Escape, JoyButton.X)
        };
    }
    public OptionsControlsViewModel(ViewModelFactory viewModelFactory, MainViewModel mainViewModel, UserInterface currentUserInterface, UserInterface dialogUserInterface, KeyRepeater keyRepeater)
    {
        _viewModelFactory = viewModelFactory;
        _mainViewModel = mainViewModel;
        _currentUserInterface = currentUserInterface;
        _dialogUserInterface = dialogUserInterface;
        _keyRepeater = keyRepeater;

        HashSet<Key> navigationReservedKeys = new()
        {
            Key.Escape,
            Key.Enter,
            Key.Space
        };
        var navigationGroup = new InputMapGroup(navigationReservedKeys);
        InputMapItems = new()
        {
            new("ui_accept", "Confirm", navigationGroup),
            new("ui_cancel", "Cancel", navigationGroup),
            new("ui_left", "Left", navigationGroup),
            new("ui_right", "Right", navigationGroup),
            new("ui_up", "Up", navigationGroup),
            new("ui_down", "Down", navigationGroup)
        };
    }

    [RelayCommand]
    public void InputPrompt(InputMapItem inputMapItem)
    {
        var dialog = new InputListenerDialogViewModel(_dialogUserInterface, inputMapItem.ReservedKeys);
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

    public void TryClose(Action callOnClose) => callOnClose();
}
