using Avalonia.Controls;
using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.Models;
using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;

using static AudioManager;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsAudioViewModel : ViewModel, IOptionsTabViewModel
{
    [ObservableProperty]
    private int _masterLevel;
    [ObservableProperty]
    private int _musicLevel;
    [ObservableProperty]
    private int _soundEffectsLevel;
    [ObservableProperty]
    private int _interfaceLevel;

    private Options _options;

    private readonly MainViewModel _mainViewModelDialog;
    private readonly FocusStack _focusStack;
    private readonly UserInterface _dialogUserInterface;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (Design.IsDesignMode)
            return;

        switch (e.PropertyName)
        {
            case nameof(MasterLevel):
                UpdateBusDbLevelFromLinear(Bus.Master, MasterLevel);
                break;
            case nameof(MusicLevel):
                UpdateBusDbLevelFromLinear(Bus.Music, MusicLevel);
                break;
            case nameof(SoundEffectsLevel):
                UpdateBusDbLevelFromLinear(Bus.SFX, SoundEffectsLevel);
                break;
            case nameof(InterfaceLevel):
                UpdateBusDbLevelFromLinear(Bus.UI, InterfaceLevel);
                break;
        }
    }

    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public OptionsAudioViewModel() { }
    public OptionsAudioViewModel(Options options, FocusStack focusStack, UserInterface dialogUserInterface, MainViewModel mainViewModelDialog)
    {
        _options = options;
        _focusStack = focusStack;
        _dialogUserInterface = dialogUserInterface;
        _mainViewModelDialog = mainViewModelDialog;

        MasterLevel = GetBusLinearEnergyPercentage(Bus.Master);
        MusicLevel = GetBusLinearEnergyPercentage(Bus.Music);
        SoundEffectsLevel = GetBusLinearEnergyPercentage(Bus.SFX);
        InterfaceLevel = GetBusLinearEnergyPercentage(Bus.UI);
    }

    [RelayCommand]
    public void ResetToDefault()
    {
        var dialog = new DialogViewModel(
            "Are you sure you want to reset the audio levels to their defaults?\n" +
            "Any made changes will be lost.",
            "Cancel", confirmText: "Reset to default"
            );
        dialog.Responded += OnResponse;

        void OnResponse(DialogViewModel.Response response)
        {
            dialog.Responded -= OnResponse;
            if (response == DialogViewModel.Response.Confirm)
            {
                MasterLevel = 100;
                MusicLevel = 100;
                SoundEffectsLevel = 100;
                InterfaceLevel = 100;
            }
        }

        _mainViewModelDialog.NavigateTo(dialog);
        _focusStack.Push(_dialogUserInterface);
        dialog.Closed += (_) => _focusStack.Pop();
    }

    public void TryClose(Action callOnClose)
    {
        _options.AudioOptions = new AudioOptions()
        {
            MasterLevel = MasterLevel,
            MusicLevel = MusicLevel,
            SoundEffectsLevel = SoundEffectsLevel,
            InterfaceLevel = InterfaceLevel
        };
        _options.Save();

        callOnClose();
    }
}
