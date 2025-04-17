using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.Models;
using Godot;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class EscapeMenuViewModel : ViewModel
{
    private readonly ViewModelFactory _viewModelFactory;
    private readonly NavigatorViewModel _navigatorViewModel;
    private readonly FocusStack _focusStack;
    private readonly UserInterface _dialogUserInterface;
    private readonly MainViewModel _mainViewModelDialog;

    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public EscapeMenuViewModel() { }
    public EscapeMenuViewModel(ViewModelFactory viewModelFactory, NavigatorViewModel navigatorViewModel, FocusStack focusStack, 
        UserInterface dialogUserInterface, MainViewModel mainViewModelDialog)
    {
        _viewModelFactory = viewModelFactory;
        _navigatorViewModel = navigatorViewModel;
        _focusStack = focusStack;
        _dialogUserInterface = dialogUserInterface;
        _mainViewModelDialog = mainViewModelDialog;
    }

    [RelayCommand]
    public void ToOptions()
    {
        _navigatorViewModel.NavigateTo(_viewModelFactory.CreateOptions());
    }

    [RelayCommand]
    public void ToMainMenu()
    {
        var dialog = new DialogViewModel(
            "Are you sure you want to exit to the main menu?",
            "Cancel", confirmText: "Exit"
            );
        dialog.Responded += OnResponse;

        void OnResponse(DialogViewModel.Response response)
        {
            dialog.Responded -= OnResponse;
            if (response == DialogViewModel.Response.Confirm)
            {
                _navigatorViewModel.NavigateTo(_viewModelFactory.CreateMainMenu(), clearStack: true);
            }
        }

        _mainViewModelDialog.NavigateTo(dialog);
        _focusStack.Push(_dialogUserInterface);
        dialog.Closed += _focusStack.Pop;
    }
}
