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
    public event EventHandler<ViewModel?>? Navigated;

    [ObservableProperty]
    private ViewModel? _currentViewModel;

    [ObservableProperty]
    private IPageTransition? _transition = null;

    private Utilities.PageTransitionWithDuration? _pageTransition;
    private readonly Stack<ViewModel> _viewModels = new();

    protected readonly UserInterface _userInterface;

    public NavigatorViewModel(UserInterface userInterface)
    {
        _userInterface = userInterface;
    }

    private void OnViewModelsAddedOrRemoved()
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

    protected virtual void OnNavigated()
    {
        Navigated?.Invoke(this, CurrentViewModel);
    }

    public async void DisableInputForTransitionDuration(Utilities.PageTransitionWithDuration transition)
    {
        _userInterface.InputEnabled = false;

        await transition.StartToEnd();
        _userInterface.InputEnabled = true;
    }

    public void NavigateTo(ViewModel viewModel, Utilities.PageTransitionWithDuration? transition = null, bool replace = false, bool clearStack = false)
    {
        _pageTransition = transition;
        this.Transition = transition;

        if (clearStack)
        {
            _viewModels.Clear();
        }
        else if (replace)
        {
            _viewModels.Pop();
        }

        viewModel.Closed += OnViewModelClosed;
        _viewModels.Push(viewModel);
        CurrentViewModel = viewModel;

        OnViewModelsAddedOrRemoved();
        OnNavigated();

        if (_pageTransition != null)
        {
            DisableInputForTransitionDuration(_pageTransition);
        }


        void OnViewModelClosed()
        {
            viewModel.Closed -= OnViewModelClosed;
            _viewModels.Pop();
            if (_viewModels.Count > 0)
            {
                CurrentViewModel = _viewModels.Peek();
                CurrentViewModel.OnNavigatorFocusReturned();
            }
            else
            {
                CurrentViewModel = null;
            }

            OnViewModelsAddedOrRemoved();
            OnNavigated();

            if (_pageTransition != null)
            {
                DisableInputForTransitionDuration(_pageTransition);
            }
        }
    }
}
