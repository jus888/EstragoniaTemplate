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

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsControlsViewModel : ViewModel, IOptionsTabViewModel
{
    public ObservableCollection<InputMapItem> InputMapItems { get; set; }

    public OptionsControlsViewModel()
    {
        InputMapItems = new()
        {
            new("", "Confirm", Key.Enter, JoyButton.A),
            new("", "Cancel", Key.Escape, JoyButton.X)
        };
    }

    public void TryClose(Action callOnClose) => callOnClose();
}
