using Avalonia.Animation;
using CommunityToolkit.Mvvm.ComponentModel;
using EstragoniaTemplate.Main;
using System;
using System.Collections.Generic;
using EstragoniaTemplate.UI;
using System.Transactions;

namespace EstragoniaTemplate.UI.ViewModels;

public abstract partial class NavigatorViewModel : ViewModel
{
    [ObservableProperty]
    private ViewModel? _currentViewModel;

    [ObservableProperty]
    private IPageTransition? _transition = null;

    protected readonly Stack<ViewModel> _viewModels = new();
    protected readonly UserInterface _userInterface;

    private Utilities.PageTransitionWithDuration? _pageTransition;

    public NavigatorViewModel(UserInterface userInterface)
    {
        _userInterface = userInterface;
    }

    protected virtual void OnViewModelsAddedOrRemoved()
    {
        if (_userInterface == null)
            return;

        _userInterface.FocusMode = Godot.Control.FocusModeEnum.All;
        if (_viewModels.Count == 0)
        {
            _userInterface.FocusMode = Godot.Control.FocusModeEnum.None;
            _userInterface.ReleaseFocus();
        }
    }

    public void NavigateTo(ViewModel viewModel, Utilities.PageTransitionWithDuration? transition = null, bool replace = false, bool clearStack = false)
    {
        _pageTransition = transition;
        this.Transition = transition;

        if (clearStack)
        {
            while (CurrentViewModel != null)
            {
                CurrentViewModel.ForcedClose();
            }
        }
        else if (replace)
        {
            CurrentViewModel?.ForcedClose();
        }

        viewModel.Closed += OnViewModelClosed;
        _viewModels.Push(viewModel);
        CurrentViewModel = viewModel;

        OnViewModelsAddedOrRemoved();

        if (_pageTransition != null)
        {
            DisableInputForTransitionDuration(_pageTransition);
        }


        void OnViewModelClosed(bool forced)
        {
            viewModel.Closed -= OnViewModelClosed;
            _viewModels.Pop();

            if (_viewModels.Count > 0)
            {
                CurrentViewModel = _viewModels.Peek();
            }
            else
            {
                CurrentViewModel = null;
            }

            if (forced)
                return;

            CurrentViewModel?.OnNavigatorFocusReturned();
            OnViewModelsAddedOrRemoved();

            if (_pageTransition != null)
            {
                DisableInputForTransitionDuration(_pageTransition);
            }
        }
    }

    public async void DisableInputForTransitionDuration(Utilities.PageTransitionWithDuration transition)
    {
        _userInterface.InputEnabled = false;

        await transition.StartToEnd();
        _userInterface.InputEnabled = true;
    }

    public override void Close()
    {
        while (CurrentViewModel != null)
        {
            CurrentViewModel.ForcedClose();
        }
        base.Close();
    }

    public override void OnNavigatorFocusReturned()
    {
        base.OnNavigatorFocusReturned();
        CurrentViewModel?.OnNavigatorFocusReturned();
    }

    public override void OnUserInterfaceFocusReturned()
    {
        base.OnUserInterfaceFocusReturned();
        CurrentViewModel?.OnUserInterfaceFocusReturned();
    }

    public override void OnUserInterfaceFocusLost()
    {
        base.OnUserInterfaceFocusLost();
        CurrentViewModel?.OnUserInterfaceFocusLost();
    }
}
