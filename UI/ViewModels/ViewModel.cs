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

    /// <summary>
    /// If argument is true: closed forcefully (by the navigator).
    /// </summary>
    public event Action<bool>? Closed;

    private bool _forcedClose = false;

    [RelayCommand]
    public virtual void Close()
    {
        Closed?.Invoke(_forcedClose);
    }

    public void ForcedClose()
    {
        _forcedClose = true;
        Close();
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
