using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI.Controls;

public class SequentialFade : IPageTransition
{
    private readonly Animation _fadeOutAnimation;
    private readonly Animation _fadeInAnimation;

    public SequentialFade() : this(TimeSpan.Zero) { }
    public SequentialFade(TimeSpan duration)
    {
        _fadeOutAnimation = new Animation
        {
            Children =
                {
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 1d
                            }
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(0.5d)
                    },
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(1d)
                    }
                }
        };
        _fadeInAnimation = new Animation
        {
            Children =
                {
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(0.5d)
                    },
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 1d
                            }
                        },
                        Cue = new Cue(1d)
                    }

                }
        };
        _fadeOutAnimation.Duration = _fadeInAnimation.Duration = duration;
    }

    /// <summary>
    /// Gets the duration of the animation.
    /// </summary>
    public TimeSpan Duration
    {
        get => _fadeOutAnimation.Duration;
        set => _fadeOutAnimation.Duration = _fadeInAnimation.Duration = value;
    }

    /// <summary>
    /// Gets or sets element entrance easing.
    /// </summary>
    public Easing FadeInEasing
    {
        get => _fadeInAnimation.Easing;
        set => _fadeInAnimation.Easing = value;
    }

    /// <summary>
    /// Gets or sets element exit easing.
    /// </summary>
    public Easing FadeOutEasing
    {
        get => _fadeOutAnimation.Easing;
        set => _fadeOutAnimation.Easing = value;
    }

    /// <inheritdoc cref="Start(Visual, Visual, CancellationToken)" />
    public async Task Start(Visual? from, Visual? to, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var tasks = new List<Task>();

        if (from != null)
        {
            tasks.Add(_fadeOutAnimation.RunAsync(from, cancellationToken));
        }

        if (to != null)
        {
            to.IsVisible = true;
            tasks.Add(_fadeInAnimation.RunAsync(to, cancellationToken));
        }

        await Task.WhenAll(tasks);

        if (from != null && !cancellationToken.IsCancellationRequested)
        {
            from.IsVisible = false;
        }
    }

    /// <summary>
    /// Starts the animation.
    /// </summary>
    /// <param name="from">
    /// The control that is being transitioned away from. May be null.
    /// </param>
    /// <param name="to">
    /// The control that is being transitioned to. May be null.
    /// </param>
    /// <param name="forward">
    /// Unused for cross-fades.
    /// </param>
    /// <param name="cancellationToken">allowed cancel transition</param>
    /// <returns>
    /// A <see cref="Task"/> that tracks the progress of the animation.
    /// </returns>
    Task IPageTransition.Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        return Start(from, to, cancellationToken);
    }
}
