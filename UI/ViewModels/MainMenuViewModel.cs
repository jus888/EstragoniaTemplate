using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainMenuViewModel : ViewModel
{
    private readonly MainViewModel? _mainViewModel;

    public MainMenuViewModel() { }
    public MainMenuViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    public void ToOptions()
    {
        _mainViewModel?.NavigateTo(new OptionsViewModel());
    }
}
