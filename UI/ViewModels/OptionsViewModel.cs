using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.UI.Models;
using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsViewModel : ViewModel
{
    [ObservableProperty]
    private UIOptions _options;
    private UIOptions _originalOptions;
    private UIOptions _currentlyAppliedOptions;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
    private bool _canApply = false;

    // Nullable because the view uses constructor with only UIOptions (for the designer).
    private readonly MainViewModel? _mainViewModel;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName != nameof(CanApply))
        {
            CanApply = !Options.Equals(_currentlyAppliedOptions);
        }
    }

    public OptionsViewModel(MainViewModel mainViewModel) : this(mainViewModel.UIOptions)
    {
        _mainViewModel = mainViewModel;
    }
    public OptionsViewModel(UIOptions options)
    {
        _currentlyAppliedOptions = new(options);
        Options = options;
        Options.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(e);
        };

        _originalOptions = new(options);
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
        if (!Options.Equals(_originalOptions))
        {
            Options.SaveOverrideFile();

            using var file = FileAccess.Open("user://settings.json", FileAccess.ModeFlags.Write);
            file.StoreString(JsonSerializer.Serialize(Options));
        }

        if (CanApply)
        {
            Apply();
        }

        Close();
    }

    [RelayCommand]
    public void Exit()
    {
        if (!_currentlyAppliedOptions.Equals(_originalOptions))
        {
            var dialog = new DialogViewModel("You have unsaved applied changes.\nExit without saving or save changes?", "Cancel", "Do not save", "Save applied settings");
            dialog.UserResponded += OnResponse;

            void OnResponse(DialogViewModel.Response response)
            {
                dialog.UserResponded -= OnResponse;
                switch (response)
                {
                    case DialogViewModel.Response.Cancel:
                        return;
                    case DialogViewModel.Response.Deny:
                        Options.SetFromOptions(_originalOptions);
                        Apply();
                        Close();
                        return;
                    case DialogViewModel.Response.Confirm:
                        Options.SetFromOptions(_currentlyAppliedOptions);
                        Save();
                        return;
                }
            }

            _mainViewModel?.ShowDialogViewModel(dialog);
        }
        else
        {
            Options.SetFromOptions(_originalOptions);
            Apply();
            Close();
        }
    }
}
