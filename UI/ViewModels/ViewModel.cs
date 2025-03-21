using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace EstragoniaTemplate.UI.ViewModels;

public abstract partial class ViewModel : ObservableObject
{
    /// <summary>
    /// Invoked when this view becomes the top of the stack after another view is popped.
    /// </summary>
    public event EventHandler? NavigatorFocusReturned;

    public event EventHandler? UserInterfaceFocusReturned;
    public event EventHandler? UserInterfaceFocusLost;
    public event Action? Closed;

    [RelayCommand]
    public virtual void Close()
    {
        Closed?.Invoke();
    }

    public virtual void OnNavigatorFocusReturned()
    {
        NavigatorFocusReturned?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnUserInterfaceFocusReturned()
    {
        UserInterfaceFocusReturned?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnUserInterfaceFocusLost()
    {
        UserInterfaceFocusLost?.Invoke(this, EventArgs.Empty);
    }
}
