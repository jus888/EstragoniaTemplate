using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainViewModel : ViewModel
{
    ViewModel? CurrentViewModel
    {
        get => _viewModels.Count > 0 ? _viewModels.Peek() : null;
    }

    private readonly Stack<ViewModel> _viewModels = new();

    public void NavigateTo(ViewModel viewModel, bool clearStack = false)
    {
        if (clearStack)
        {
            _viewModels.Clear();
        }

        viewModel.Closed += OnViewModelClosed;
        _viewModels.Push(viewModel);
        OnPropertyChanged(nameof(CurrentViewModel));

        void OnViewModelClosed()
        {
            viewModel.Closed -= OnViewModelClosed;

            _viewModels.Pop();
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
