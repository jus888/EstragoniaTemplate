using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using System;
using EstragoniaTemplate.Main;

namespace EstragoniaTemplate.UI.ViewModels;

public interface IOptionsTabViewModel
{
    public void TryClose(Action callOnClose);
}

public partial class OptionsViewModel : NavigatorViewModel
{
    [ObservableProperty]
    private int _currentTabIndex = 0;

    private ViewModelFactory _viewModelFactory;

    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public OptionsViewModel(ViewModel initialViewModel) : base(null)
    {
        NavigateTo(initialViewModel);
    }

    public OptionsViewModel(ViewModelFactory viewModelFactory, UserInterface userInterface) : base(userInterface)
    {
        _viewModelFactory = viewModelFactory;
        NavigateTo(viewModelFactory.CreateOptionsGraphics());
        ToOptionsTabCommand.NotifyCanExecuteChanged();
    }

    public enum OptionsTab
    {
        Graphics = 0,
        Controls = 1
    }

    [RelayCommand]
    public void ToOptionsTab(int tabIndex)
    {
        if (tabIndex == CurrentTabIndex)
            return;

        var tab = (OptionsTab)tabIndex;
        Action action;
        if (tab == OptionsTab.Graphics)
        {
            action = () =>
            {
                NavigateTo(_viewModelFactory.CreateOptionsGraphics(), replace: true);
                ToOptionsTabCommand.NotifyCanExecuteChanged();
                CurrentTabIndex = tabIndex;
            };
        }
        else/* if (tab == OptionsTab.Controls)*/
        {
            action = () =>
            {
                NavigateTo(_viewModelFactory.CreateOptionsControls(), replace: true);
                ToOptionsTabCommand.NotifyCanExecuteChanged();
                CurrentTabIndex = tabIndex;
            };
        }

        if (CurrentViewModel is IOptionsTabViewModel viewModel)
        {
            viewModel.TryClose(action);
        }
    }

    public override void Close()
    {
        if (CurrentViewModel is IOptionsTabViewModel viewModel)
        {
            viewModel.TryClose(() => base.Close());
        }
    }
}
