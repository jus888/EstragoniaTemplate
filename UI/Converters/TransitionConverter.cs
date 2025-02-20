using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Metadata;
using EstragoniaTemplate.UI.ViewModels;
using EstragoniaTemplate.UI.DataTemplates;

namespace EstragoniaTemplate.UI.Converters;

public class TransitionConverter : IValueConverter
{
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTransitions { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TransitionTemplateArguments args)
        {
            return AvailableTransitions[args.Type.ToString()].Build(parameter);
        }

        return new BindingNotification(new ArgumentException(nameof(value)), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => BindingOperations.DoNothing;

    //public Control? Build(object? param)
    //{
    //    var args = param as TransitionTemplateArguments;
    //    if (args == null)
    //    {
    //        throw new ArgumentNullException(nameof(param));
    //    }
    //    return AvailableTransitions[args.Type.ToString()].Build(param);
    //}

    //public bool Match(object? data)
    //{
    //    return data is TransitionTemplateArguments;
    //}
}
