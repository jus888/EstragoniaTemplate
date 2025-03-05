using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace EstragoniaTemplate.UI.ViewModels;

public class NavigatorReturnedFocusEventArgs : EventArgs
{
    public bool ChangedUserInterface { get; set; } = false;
}

public abstract partial class ViewModel : ObservableObject
{
    public event EventHandler? NavigatorReturnedFocus;
    public event Action? Closed;

    [RelayCommand]
    public void Close()
    {
        Closed?.Invoke();
    }

    public virtual void OnNavigatorReturnedFocus(bool changedUserInterface = false)
    {
        NavigatorReturnedFocus?.Invoke(this, new NavigatorReturnedFocusEventArgs
        {
            ChangedUserInterface = changedUserInterface
        });
    }
}
