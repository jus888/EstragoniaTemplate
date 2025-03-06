using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using System;
using Godot;

using static EstragoniaTemplate.UI.Utilities;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainMenuViewModel : ViewModel
{
    private readonly MainViewModel _mainViewModel;
    private readonly Node _node;

    public MainMenuViewModel() { }
    public MainMenuViewModel(MainViewModel mainViewModel, Node node)
    {
        _mainViewModel = mainViewModel;
        _node = node;
    }

    [RelayCommand]
    public void ToOptions()
    {
        _mainViewModel?.NavigateTo(new OptionsViewModel(_mainViewModel), 
            CreateCommonPageTransition(TransitionType.Fade, 0.5f));
    }

    [RelayCommand]
    public void Quit()
    {
        _node.GetTree().Quit();
    }
}
