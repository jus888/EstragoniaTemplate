using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using EstragoniaTemplate.UI.ViewModels;
using System;
using System.Collections.Generic;
using static EstragoniaTemplate.UI.Utilities;

namespace EstragoniaTemplate.UI.Views;

public abstract partial class View : UserControl
{
    public bool Test { get; set; }

    protected LinkedList<Control> LastFocussedControls = new();
    protected const int FocussedTrackCount = 2;

    private bool _trackFocussedControls = true;

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);

        if (_trackFocussedControls && e.Source is Control control && control.IsFocused)
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
        viewModel.NavigatorFocusReturned += (s, e) => FocusLast();

        viewModel.UserInterfaceFocusReturned += (s, e) =>
        {
            FocusLast();
            _trackFocussedControls = true;
        };
        viewModel.UserInterfaceFocusLost += (s, e) =>
        {
            FocusLast();
            _trackFocussedControls = false;
        };

        void FocusLast()
        {
            var lastNode = LastFocussedControls.Last;
            lastNode?.Value.Focus(NavigationMethodBasedOnMouseOrKey);
        }
    }
    public void FocusNamedControls()
    {
        Control? focusableControl = null;
        int count = 0;
        while (focusableControl == null)
        {
            try
            {
                var control = this.GetControl<Control>($"initialFocus{count}");
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

        focusableControl?.Focus(NavigationMethodBasedOnMouseOrKey);
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
            nextFocus.Focus(NavigationMethodBasedOnMouseOrKey);
            AudioManager.Instance?.Play(this, AudioManager.Sound.UISelect, AudioManager.Bus.UI);
            e.Handled = true;
        }
    }
}
