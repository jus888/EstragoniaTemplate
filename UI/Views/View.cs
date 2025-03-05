using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using EstragoniaTemplate.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EstragoniaTemplate.UI.Views;

public abstract partial class View : UserControl
{
    public bool Test { get; set; }

    protected LinkedList<Control> LastFocussedControls = new();
    protected const int FocussedTrackCount = 2;

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);

        if (e.Source is Control control && control.IsFocused)
        {
            LastFocussedControls.AddLast(control);
            if (LastFocussedControls.Count > FocussedTrackCount)
            {
                LastFocussedControls.RemoveFirst();
            }
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        FocusNamedControls();

        if (DataContext == null)
            return;

        var viewModel = (ViewModel)DataContext;
        viewModel.NavigatorReturnedFocus += (s, e) =>
        {
            var lastNode = LastFocussedControls.Last;
            var args = (NavigatorReturnedFocusEventArgs)e;
            if (args.ChangedUserInterface)
            {
                lastNode?.Previous?.Value.Focus(NavigationMethod.Directional);
            }
            else
            {
                lastNode?.Value.Focus(NavigationMethod.Directional);
            }
        };
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

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled || e.KeyModifiers != KeyModifiers.None || e.Source is not InputElement inputElement)
            return;

        IInputElement? nextFocus = null;
        switch (e.Key)
        {
            case Key.Up:
                nextFocus = KeyboardNavigationHandler.GetNext(inputElement, NavigationDirection.Up);
                break;
            case Key.Down:
                nextFocus = KeyboardNavigationHandler.GetNext(inputElement, NavigationDirection.Down);
                break;
            case Key.Left:
                nextFocus = KeyboardNavigationHandler.GetNext(inputElement, NavigationDirection.Left);
                break;
            case Key.Right:
                nextFocus = KeyboardNavigationHandler.GetNext(inputElement, NavigationDirection.Right);
                break;
        }

        if (nextFocus != null && nextFocus.Focusable)
        {
            nextFocus.Focus(NavigationMethod.Directional);
        }
    }
}
