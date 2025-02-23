using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.UI.Models;
using Godot;
using System.ComponentModel;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsViewModel : ViewModel
{
    [ObservableProperty]
    UIOptions _options;

    private bool _canApply = false;
    public bool CanApply
    {
        get => _canApply;
        private set
        {
            if (SetProperty(ref _canApply, value))
            {
                ApplyCommand.NotifyCanExecuteChanged();
            }
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        CanApply = true;
    }

    public OptionsViewModel(UIOptions uiOptions)
    {
        _options = uiOptions;
    }

    [RelayCommand(CanExecute = nameof(CanApply))]
    public void Apply()
    {
        _canApply = false;

        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
        DisplayServer.WindowSetMode(Options.WindowMode);
        DisplayServer.WindowSetVsyncMode(Options.VSync ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);

        AvaloniaLoader.Instance.UIScalingOption = Options.UIScale;

        Engine.MaxFps = 0;
        if (!Options.VSync)
        {
            Engine.MaxFps = Options.FPSLimit;
        }
    }
}
