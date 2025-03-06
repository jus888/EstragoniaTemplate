using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using Godot;
using System;
using System.Diagnostics;
using EstragoniaTemplate.UI;
using Avalonia.Input;

namespace EstragoniaTemplate.UI;

public static class Utilities
{
    public static NavigationMethod NavigationMethodBasedOnMouseOrKey()
        => AvaloniaLoader.LastPressedInputWasMouseClick ? NavigationMethod.Unspecified : NavigationMethod.Directional;

    /// <summary>
    /// Note: rounds to milliseconds.
    /// </summary>
    public static TimeSpan CreateTimeSpanSeconds(float seconds)
    {
        return new TimeSpan(0, 0, 0, 0, Mathf.RoundToInt(seconds * 1000));
    }

    public enum TransitionType
    {
        Fade
    }

    public static IPageTransition? CreateCommonPageTransition(TransitionType transitionType, float durationSeconds)
    {
        var duration = CreateTimeSpanSeconds(durationSeconds);

        IPageTransition? transition = null;
        switch (transitionType)
        {
            case TransitionType.Fade:
                transition = new CompositePageTransition() 
                { 
                    PageTransitions = new() 
                    { 
                        new SequentialFade(duration), 
                        new TransitionDisableFromControl(duration) 
                    }
                };
                break;
        }

        return transition;
    }
}