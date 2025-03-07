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
    private UserInterface _userInterface;
    private UserInterface _userInterfaceDialog;

    public ViewModelFactory(UIOptions uiOptions, MainViewModel mainViewModel, UserInterface userInterface, UserInterface userInterfaceDialog)
    {
        _options = uiOptions;
        _mainViewModel = mainViewModel;
        _userInterface = userInterface;
        _userInterfaceDialog = userInterfaceDialog;
    }

    public MainMenuViewModel CreateMainMenu(SceneTree sceneTree)
        => new MainMenuViewModel(this, _mainViewModel, sceneTree);

    public OptionsViewModel CreateOptions()
        => new OptionsViewModel(_options, _userInterface, _userInterfaceDialog);
}
