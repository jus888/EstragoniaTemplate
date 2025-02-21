using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using System;
using static EstragoniaTemplate.UI.Utilities;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainMenuViewModel : ViewModel
{
    private readonly MainViewModel? _mainViewModel;

    public MainMenuViewModel() { }
    public MainMenuViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    public void ToOptions()
    {
        _mainViewModel?.NavigateTo(new OptionsViewModel(), 
            CreateCommonTransition(TransitionType.Fade, 0.5f));
    }
}
