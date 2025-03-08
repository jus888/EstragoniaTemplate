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
    private UIOptions _options;
    private UIOptions _savedOptions;
    private UIOptions _currentlyAppliedOptions;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
    private bool _canApply = false;

    private readonly MainViewModel _mainViewModel;
    private readonly UserInterface? _currentUserInterface;
    private readonly UserInterface? _targetUserInterface;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName != nameof(CanApply))
        {
            CanApply = !Options.Equals(_currentlyAppliedOptions);
        }
    }

    /// <summary>
    /// Set the UserInterface parameters if dialog should open in a different UserInterface (target).
    /// </summary>
    public OptionsGraphicsViewModel(UIOptions options, MainViewModel mainViewModel, UserInterface? currentUserInterface = null, UserInterface? targetUserInterface = null) : this(options)
    {
        _mainViewModel = mainViewModel;
        _currentUserInterface = currentUserInterface;
        _targetUserInterface = targetUserInterface;
    }
    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public OptionsGraphicsViewModel(UIOptions options)
    {
        _savedOptions = new(options);
        _currentlyAppliedOptions = new(options);

        Options = options;
        Options.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(e);
        };
    }

    [RelayCommand(CanExecute = nameof(CanApply))]
    public void Apply()
    {
        Options.Apply();
        CanApply = false;
        _currentlyAppliedOptions = new(Options);
    }

    [RelayCommand]
    public void Save()
    {
        if (!Options.Equals(_savedOptions))
        {
            Options.SaveOverrideFile();

            using var file = FileAccess.Open("user://settings.json", FileAccess.ModeFlags.Write);
            file.StoreString(JsonSerializer.Serialize(Options));
            _savedOptions = new UIOptions(Options);
        }

        if (CanApply)
        {
            Apply();
        }
    }

    public void TryClose(Action callOnClose)
    {
        if (!_currentlyAppliedOptions.Equals(_savedOptions))
        {
            var dialog = new DialogViewModel("You have unsaved applied changes.\nExit without saving or save changes?", "Cancel", "Do not save", "Save applied settings");
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
                        Apply();
                        callOnClose();
                        return;
                    case DialogViewModel.Response.Confirm:
                        Options.SetFromOptions(_currentlyAppliedOptions);
                        Save();
                        callOnClose();
                        return;
                }
            }

            _mainViewModel.NavigateTo(dialog);

            if (_currentUserInterface != null && _targetUserInterface != null)
            {
                _targetUserInterface.StealFocus(_currentUserInterface, true);
            }
        }
        else
        {
            Options.SetFromOptions(_savedOptions);
            callOnClose();
        }
    }
}
