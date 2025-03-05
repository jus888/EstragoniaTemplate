using Avalonia.Animation;
using CommunityToolkit.Mvvm.ComponentModel;
using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainViewModel : NavigatorViewModel
{
    public UIOptions UIOptions { get; set; }
    public MainViewModel? DialogMainViewModel { get; set; }

    public MainViewModel(UserInterface userInterface, UIOptions uiOptions, MainViewModel? dialogMainViewModel = null) : base(userInterface)
    {
        UIOptions = uiOptions;
        DialogMainViewModel = dialogMainViewModel;
    }

    public void GrabFocus()
    {
        _userInterface.GrabFocus();
    }

    public void ShowDialogViewModel(DialogViewModel dialog)
    {
        if (DialogMainViewModel == null) throw new InvalidOperationException("Cannot show dialog: DialogMainViewModel is null.");

        _userInterface.FocusMode = Control.FocusModeEnum.None;
        _userInterface.ReleaseFocus();

        DialogMainViewModel.NavigateTo(dialog);
        DialogMainViewModel.GrabFocus();

        dialog.UserResponded += OnResponse;

        void OnResponse(DialogViewModel.Response _)
        {
            dialog.UserResponded -= OnResponse;
            _userInterface.FocusMode = Control.FocusModeEnum.All;
            _userInterface.GrabFocus();
            CurrentViewModel?.OnNavigatorReturnedFocus(changedUserInterface: true);
        }
    }
}
