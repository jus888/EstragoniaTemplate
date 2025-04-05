﻿using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace EstragoniaTemplate.UI.Controls;

[TemplatePart("PART_Grid", typeof(Grid))]
public class FocusableSlider : Slider
{
    protected override Type StyleKeyOverride => typeof(Slider);

    private Grid? _grid;
    private bool _focusEngaged = false;
    private bool FocusEngaged
    {
        get => _focusEngaged;
        set
        {
            if (_focusEngaged != value)
            {
                if (_focusEngaged)
                {
                    _grid?.Classes.Remove("engaged");
                }
                else
                {
                    _grid?.Classes.Add("engaged");
                }
            }
            _focusEngaged = value;
        }
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        FocusEngaged = false;
        base.OnLostFocus(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e is { KeyModifiers: KeyModifiers.None, Key: Key.Up or Key.Down })
            return;

        if (e.Key == Key.Enter)
        {
            FocusEngaged = !FocusEngaged;
        }

        if (!_focusEngaged)
            return;

        base.OnKeyDown(e);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _grid = e.NameScope.Find("PART_Grid") as Grid;
    }
}
