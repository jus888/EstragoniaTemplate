using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;

namespace EstragoniaTemplate.UI.Views;

public abstract partial class View : UserControl
{
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        FocusNamedControls();
    }

    public void FocusNamedControls()
    {
        Control? focusableControl = null;
        int count = 0;
        while (focusableControl == null)
        {
            try
            {
                var control = this.GetControl<Control>($"InitialFocus{count}");
                if (control.Focusable)
                {
                    focusableControl = control;
                }
            }
            catch (ArgumentException)
            {
                break;
            }

            count++;
        }

        focusableControl?.Focus(NavigationMethod.Directional);
    }
}
