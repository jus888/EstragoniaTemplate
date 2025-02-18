using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    ViewModelBase? _currentViewModel;

    private Stack<ViewModelBase> _viewModels = new();

    public MainViewModel(ViewModelBase initialViewModel)
    {
        _currentViewModel = initialViewModel;
    }
}
