using Avalonia;
using Avalonia.Animation;
using Avalonia.Input;
using Avalonia.Platform;
using EstragoniaTemplate.UI.Controls;
using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI;

public static class Utilities
{
    public static Avalonia.Media.Imaging.Bitmap LoadImageFromResource(Uri resourceUri)
    {
        return new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(resourceUri));
    }

    public static NavigationMethod NavigationMethodBasedOnMouseOrKey
        => AvaloniaLoader.LastPressedInputWasMouseClick ? NavigationMethod.Unspecified : NavigationMethod.Directional;

    /// <summary>
    /// Note: rounds to milliseconds.
    /// </summary>
    public static TimeSpan TimeSpanFromSeconds(float seconds)
    {
        return new TimeSpan(0, 0, 0, 0, Mathf.RoundToInt(seconds * 1000));
    }

    public enum TransitionType
    {
        Fade
    }

    public class PageTransitionWithDuration : IPageTransition
    {
        public IPageTransition PageTransition { get; set; }
        public float Duration { get; set; }

        public PageTransitionWithDuration(IPageTransition pageTransition, float durationSeconds)
        {
            PageTransition = pageTransition;
            Duration = durationSeconds;
        }

        public async Task StartToEnd()
        {
            await Task.Delay(TimeSpanFromSeconds(Duration));
            return;
        }

        public Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
            => PageTransition.Start(from, to, forward, cancellationToken);
    }

    public static PageTransitionWithDuration CreatePageTransition(TransitionType transitionType, float durationSeconds)
    {
        var duration = TimeSpanFromSeconds(durationSeconds);

        IPageTransition transition;
        switch (transitionType)
        {
            default:
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

        return new(transition, durationSeconds);
    }
}