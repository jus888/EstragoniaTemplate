using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using System;
using Godot;

using static EstragoniaTemplate.UI.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainMenuViewModel : ViewModel
{
    private readonly ViewModelFactory _viewModelFactory;
    private readonly NavigatorViewModel _navigatorViewModel;
    private readonly SceneTree _sceneTree;

    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public MainMenuViewModel() { }
    public MainMenuViewModel(ViewModelFactory viewModelFactory, NavigatorViewModel navigatorViewModel, SceneTree sceneTree)
    {
        _viewModelFactory = viewModelFactory;
        _navigatorViewModel = navigatorViewModel;
        _sceneTree = sceneTree;
    }

    [RelayCommand]
    public void ToOptions()
    {
        _navigatorViewModel.NavigateTo(_viewModelFactory.CreateOptions(), 
            CreateCommonPageTransition(TransitionType.Fade, 0.5f));
    }

    [RelayCommand]
    public void Quit()
    {
        _sceneTree.Quit();
    }
}
