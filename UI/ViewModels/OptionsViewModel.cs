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

    public bool DifferentFromCurrentViewModel(int tabIndex)
    {
        switch ((OptionsTab)tabIndex)
        {
            case OptionsTab.Graphics:
                return CurrentViewModel is not OptionsGraphicsViewModel;
            case OptionsTab.Controls:
                return CurrentViewModel is not OptionsControlsViewModel;
        }

        throw new ArgumentException(nameof(tabIndex));
    }

    [RelayCommand(CanExecute = nameof(DifferentFromCurrentViewModel))]
    public void ToOptionsTab(int tabIndex)
    {
        var tab = (OptionsTab)tabIndex;
        Action action;
        if (tab == OptionsTab.Graphics)
        {
            action = () =>
            {
                NavigateTo(_viewModelFactory.CreateOptionsGraphics(), replace: true);
                ToOptionsTabCommand.NotifyCanExecuteChanged();
            };
        }
        else/* if (tab == OptionsTab.Controls)*/
        {
            action = () =>
            {
                NavigateTo(new OptionsControlsViewModel(), replace: true);
                ToOptionsTabCommand.NotifyCanExecuteChanged();
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
