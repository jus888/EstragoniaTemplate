using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.Models;
using System;
using System.ComponentModel;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsGraphicsViewModel : ViewModel, IOptionsTabViewModel
{
    [ObservableProperty]
    private GraphicsOptions _graphicsOptions;
    private GraphicsOptions _savedGraphicsOptions;
    private GraphicsOptions _currentlyAppliedGraphicsOptions;

    private Options _options;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
    private bool _canApply;

    private readonly MainViewModel _mainViewModelDialog;
    private readonly FocusStack _focusStack;
    private readonly UserInterface _dialogUserInterface;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName != nameof(CanApply))
        {
            CanApply = true;
        }
    }

    public OptionsGraphicsViewModel(Options options, FocusStack focusStack, UserInterface dialogUserInterface, MainViewModel mainViewModelDialog) : this(options)
    {
        _focusStack = focusStack;
        _dialogUserInterface = dialogUserInterface;
        _mainViewModelDialog = mainViewModelDialog;
    }
    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public OptionsGraphicsViewModel(Options options)
    {
        _options = options;
        _savedGraphicsOptions = new(options.GraphicsOptions);
        _currentlyAppliedGraphicsOptions = new(options.GraphicsOptions);

        GraphicsOptions = options.GraphicsOptions;
        GraphicsOptions.PropertyChanged += OptionsPropertyChangedHandler;

        CanApply = false;
    }
    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public OptionsGraphicsViewModel() : this(new()) { }

    private void OptionsPropertyChangedHandler(object? s, PropertyChangedEventArgs e)
        => OnPropertyChanged(e);

    [RelayCommand(CanExecute = nameof(CanApply))]
    public void Apply()
    {
        GraphicsOptions.Apply();
        CanApply = false;
        _currentlyAppliedGraphicsOptions = new(GraphicsOptions);
    }

    [RelayCommand]
    public void ResetToDefault()
    {
        var dialog = new DialogViewModel(
            "Are you sure you want to reset the graphics settings to their defaults?\n" +
            "Any made changes will be lost.",
            "Cancel", confirmText: "Reset to default"
            );
        dialog.Responded += OnResponse;

        void OnResponse(DialogViewModel.Response response)
        {
            dialog.Responded -= OnResponse;
            if (response == DialogViewModel.Response.Confirm)
            {
                var defaultOptions = new GraphicsOptions();
                GraphicsOptions.SetFromOptions(defaultOptions);
                Apply();
            }
        }

        _mainViewModelDialog.NavigateTo(dialog);
        _focusStack.Push(_dialogUserInterface);
        dialog.Closed += _focusStack.Pop;
    }

    public void TryClose(Action callOnClose)
    {
        if (!_currentlyAppliedGraphicsOptions.Equals(_savedGraphicsOptions))
        {
            var dialog = new DialogViewModel(
                "You have applied changes to the graphics settings.\n" +
                "Revert the changes or save current settings?",
                "Cancel", "Revert changes", "Save current settings"
                );
            dialog.Responded += OnResponse;

            void OnResponse(DialogViewModel.Response response)
            {
                dialog.Responded -= OnResponse;
                switch (response)
                {
                    case DialogViewModel.Response.Cancel:
                        return;

                    case DialogViewModel.Response.Deny:
                        GraphicsOptions.SetFromOptions(_savedGraphicsOptions);
                        GraphicsOptions.Apply();
                        GraphicsOptions.PropertyChanged -= OptionsPropertyChangedHandler;
                        callOnClose();
                        return;

                    case DialogViewModel.Response.Confirm:
                        GraphicsOptions.SetFromOptions(_currentlyAppliedGraphicsOptions);
                        GraphicsOptions.Apply();
                        GraphicsOptions.PropertyChanged -= OptionsPropertyChangedHandler;
                        _options.Save();
                        callOnClose();
                        return;
                }
            }

            _mainViewModelDialog.NavigateTo(dialog);
            _focusStack.Push(_dialogUserInterface);
            dialog.Closed += _focusStack.Pop;
        }
        else
        {
            GraphicsOptions.SetFromOptions(_savedGraphicsOptions);
            callOnClose();
        }
    }
}
