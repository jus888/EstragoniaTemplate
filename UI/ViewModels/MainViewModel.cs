using Avalonia.Animation;
using CommunityToolkit.Mvvm.ComponentModel;
using EstragoniaTemplate.UI.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainViewModel : ViewModel
{
    [ObservableProperty]
    private ViewModel? _currentViewModel;

    [ObservableProperty]
    private IPageTransition? _transition = null;

    private readonly Stack<ViewModel> _viewModels = new();

    public UIOptions UIOptions { get; set; }

    public MainViewModel(UIOptions uiOptions)
    {
        UIOptions = uiOptions;
    }

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
