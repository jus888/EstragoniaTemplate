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
    private readonly GraphicsOptions _options;
    private readonly MainViewModel _mainViewModel;
    private readonly MainViewModel _mainViewModelDialog;
    private readonly UserInterface _userInterfaceMain;
    private readonly UserInterface _userInterfaceDialog;
    private readonly KeyRepeater _keyRepeater;

    public ViewModelFactory(GraphicsOptions uiOptions, MainViewModel mainViewModel, MainViewModel mainViewModelDialog, 
        UserInterface userInterfaceMain, UserInterface userInterfaceDialog, KeyRepeater keyRepeater)
    {
        _options = uiOptions;
        _mainViewModel = mainViewModel;
        _mainViewModelDialog = mainViewModelDialog;
        _userInterfaceMain = userInterfaceMain;
        _userInterfaceDialog = userInterfaceDialog;
        _keyRepeater = keyRepeater;
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

    public virtual OptionsControlsViewModel CreateOptionsControls()
        => new(this, _mainViewModelDialog, _userInterfaceMain, _userInterfaceDialog, _keyRepeater);
}
