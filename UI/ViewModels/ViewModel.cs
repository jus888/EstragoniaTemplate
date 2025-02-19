using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace EstragoniaTemplate.UI.ViewModels;

public abstract partial class ViewModel : ObservableObject
{
    public event Action? Closed;

    [RelayCommand]
    public void Close()
    {
        Closed?.Invoke();
    }
}
