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

    public virtual MainMenuViewModel CreateMainMenu(SceneTree sceneTree)
        => new(this, _mainViewModel, sceneTree);

    public virtual OptionsViewModel CreateOptions()
        => new(this, _userInterfaceMain);

    /// <summary>
    /// Assumes that this viewModel is created for the main UserInterface.
    /// </summary>
    public virtual OptionsGraphicsViewModel CreateOptionsGraphics()
        => new(_options, _mainViewModelDialog, _userInterfaceMain, _userInterfaceDialog);

    public virtual OptionsGraphicsViewModel CreateOptionsGraphics(MainViewModel mainViewModel)
        => new(_options, mainViewModel);
}
