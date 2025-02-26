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

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
    private bool _canApply = false;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName != nameof(CanApply))
        {
            CanApply = true;
        }
    }

    public OptionsViewModel(UIOptions uiOptions)
    {
        _options = uiOptions;
        Options.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(e);
        };

        _originalOptions = new(uiOptions);
    }

    [RelayCommand(CanExecute = nameof(CanApply))]
    public void Apply()
    {
        Options.Apply();
        CanApply = false;
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
    public void Cancel()
    {
        Options.SetFromOptions(_originalOptions);
        Apply();

        Close();
    }
}
