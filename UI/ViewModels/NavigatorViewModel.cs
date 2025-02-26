using Avalonia.Animation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public void SetPageTransition(IPageTransition? transition)
    {
        Transition = transition;
    }

    public void NavigateTo(ViewModel viewModel, IPageTransition? transition = null, bool clearStack = false)
    {
        this.Transition = transition;

        if (clearStack)
        {
            _viewModels.Clear();
        }

        viewModel.Closed += OnViewModelClosed;
        _viewModels.Push(viewModel);
        CurrentViewModel = viewModel;

        void OnViewModelClosed()
        {
            viewModel.Closed -= OnViewModelClosed;
            _viewModels.Pop();
            CurrentViewModel = _viewModels.Peek();
        }
    }
}
