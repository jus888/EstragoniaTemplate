using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using EstragoniaTemplate.UI.ViewModels;

namespace EstragoniaTemplate.UI.DataTemplates;

public class TransitionTemplateSelector : IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTransitions { get; } = new();

    public Control? Build(object? param)
    {
        var args = param as TransitionTemplateArguments;
        if (args == null)
        {
            throw new ArgumentNullException(nameof(param));
        }
        return AvailableTransitions[args.Type.ToString()].Build(param);
    }

    public bool Match(object? data)
    {
        return data is TransitionTemplateArguments;
    }
}
