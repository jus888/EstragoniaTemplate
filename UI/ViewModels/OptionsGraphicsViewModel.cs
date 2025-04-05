using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.Models;
using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsGraphicsViewModel : ViewModel, IOptionsTabViewModel
{
    [ObservableProperty]
    private GraphicsOptions _options;
    private GraphicsOptions _savedOptions;
    private GraphicsOptions _currentlyAppliedOptions;

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

    /// <summary>
    /// Set the UserInterface parameters if dialog should open in a different UserInterface (target).
    /// </summary>
    public OptionsGraphicsViewModel(GraphicsOptions options, FocusStack focusStack, UserInterface dialogUserInterface, MainViewModel mainViewModelDialog) : this(options)
    {
        _focusStack = focusStack;
        _dialogUserInterface = dialogUserInterface;
        _mainViewModelDialog = mainViewModelDialog;
    }
    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public OptionsGraphicsViewModel(GraphicsOptions options)
    {
        _savedOptions = new(options);
        _currentlyAppliedOptions = new(options);

        Options = options;
        Options.PropertyChanged += OptionsPropertyChangedHandler;

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
        Options.Apply();
        CanApply = false;
        _currentlyAppliedOptions = new(Options);
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
                Options.SetFromOptions(defaultOptions);
                Apply();
            }
        }

        _mainViewModelDialog.NavigateTo(dialog);
        _focusStack.Push(_dialogUserInterface);
        dialog.Closed += _focusStack.Pop;
    }

    public void TryClose(Action callOnClose)
    {
        if (!_currentlyAppliedOptions.Equals(_savedOptions))
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
                        Options.SetFromOptions(_savedOptions);
                        Options.Apply();
                        Options.PropertyChanged -= OptionsPropertyChangedHandler;
                        callOnClose();
                        return;

                    case DialogViewModel.Response.Confirm:
                        Options.SetFromOptions(_currentlyAppliedOptions);
                        Options.Apply();
                        Options.Save();
                        Options.PropertyChanged -= OptionsPropertyChangedHandler;
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
            Options.SetFromOptions(_savedOptions);
            callOnClose();
        }
    }
}
