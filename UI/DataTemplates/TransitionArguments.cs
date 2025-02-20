using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using EstragoniaTemplate.UI.ViewModels;

namespace EstragoniaTemplate.UI.DataTemplates;

public class TransitionTemplateArguments
{
    public enum TransitionType
    {
        None,
        Fade
    }

    public TransitionType Type { get; set; } = TransitionType.None;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;

    public TransitionTemplateArguments() { }
    public TransitionTemplateArguments(TimeSpan duration, TransitionType transitionType)
    {
        Duration = duration;
        Type = transitionType;
    }
}
