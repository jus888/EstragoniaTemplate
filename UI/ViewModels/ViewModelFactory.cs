using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI.ViewModels;

public class ViewModelFactory
{
    private UIOptions _options;
    private MainViewModel _mainViewModel;
    private MainViewModel _mainViewModelDialog;
    private UserInterface _userInterfaceMain;
    private UserInterface _userInterfaceDialog;

    public ViewModelFactory(UIOptions uiOptions, MainViewModel mainViewModel, MainViewModel mainViewModelDialog, UserInterface userInterfaceMain, UserInterface userInterfaceDialog)
    {
        _options = uiOptions;
        _mainViewModel = mainViewModel;
        _mainViewModelDialog = mainViewModelDialog;
        _userInterfaceMain = userInterfaceMain;
        _userInterfaceDialog = userInterfaceDialog;
    }

    public MainMenuViewModel CreateMainMenu(SceneTree sceneTree)
        => new MainMenuViewModel(this, _mainViewModel, sceneTree);

    /// <summary>
    /// Assumes that this viewModel is created for the main UserInterface.
    /// </summary>
    public OptionsViewModel CreateOptions()
        => new OptionsViewModel(_options, _mainViewModelDialog, _userInterfaceMain, _userInterfaceDialog);

    public OptionsViewModel CreateOptions(MainViewModel mainViewModel)
        => new OptionsViewModel(_options, mainViewModel);
}
