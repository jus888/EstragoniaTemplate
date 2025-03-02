using Avalonia.Animation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.Main;
using System;
using System.Collections.Generic;

namespace EstragoniaTemplate.UI.ViewModels;

public abstract partial class NavigatorViewModel : ViewModel
{
    [ObservableProperty]
    private ViewModel? _currentViewModel;

    [ObservableProperty]
    private IPageTransition? _transition = null;

    private readonly Stack<ViewModel> _viewModels = new();

    protected readonly UserInterface _userInterface;

    public NavigatorViewModel(UserInterface userInterface)
    {
        _userInterface = userInterface;
    }

    public void SetPageTransition(IPageTransition? transition)
    {
        Transition = transition;
    }

    private void OnViewModelsAddedOrRemoved()
    {
        _userInterface.FocusMode = Godot.Control.FocusModeEnum.All;
        if (_viewModels.Count == 0)
        {
            _userInterface.FocusMode = Godot.Control.FocusModeEnum.None;
            _userInterface.ReleaseFocus();
        }
    }

    public void NavigateTo(ViewModel viewModel, IPageTransition? transition = null, bool replace = false, bool clearStack = false)
    {
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

        void OnViewModelClosed()
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

            OnViewModelsAddedOrRemoved();
        }
    }
}
