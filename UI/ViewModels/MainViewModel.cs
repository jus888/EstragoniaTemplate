using CommunityToolkit.Mvvm.ComponentModel;
using EstragoniaTemplate.UI.DataTemplates;
using System.Collections.Generic;
using System.Diagnostics;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainViewModel : ViewModel
{
    [ObservableProperty]
    private ViewModel? _currentViewModel;

    [ObservableProperty]
    private TransitionTemplateArguments _transitionArguments = new();

    private readonly Stack<ViewModel> _viewModels = new();

    public void NavigateTo(ViewModel viewModel, TransitionTemplateArguments? transitionArguments = null, bool clearStack = false)
    {
        transitionArguments ??= new();
        TransitionArguments = transitionArguments;

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
