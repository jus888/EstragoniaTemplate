using Avalonia;
using Avalonia.Data;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Utilities;
using Avalonia.Controls.Metadata;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EstragoniaTemplate.UI.Controls;

[TemplatePart("PART_TextPresenter", typeof(TextBlock))]
[TemplatePart("PART_ValueDecrementer", typeof(Button))]
[TemplatePart("PART_ValueIncrementer", typeof(Button))]
internal class HorizontalSelect : TemplatedControl
{
    private TextBlock? _textPresenter;
    private Button? _valueDecrementer;
    private Button? _valueIncrementer;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_valueDecrementer != null)
        {
            _valueDecrementer.Click -= DecrementValue;
            _valueIncrementer!.Click -= IncrementValue;
        }

        _valueDecrementer = e.NameScope.Find("PART_ValueDecrementer") as Button;
        _valueIncrementer = e.NameScope.Find("PART_ValueIncrementer") as Button;
        _textPresenter = e.NameScope.Find("PART_TextPresenter") as TextBlock;

        if (_valueDecrementer != null && _valueIncrementer != null)
        {
            _valueDecrementer.Click += DecrementValue;
            _valueIncrementer.Click += IncrementValue;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == NumberOfValuesProperty)
        {
            Values = Enumerable.Range(0, NumberOfValues);
        }
    }

    private void DecrementValue(object? sender, RoutedEventArgs e)
        => Value = MathUtilities.Clamp(Value - 1, 0, NumberOfValues - 1);
    private void IncrementValue(object? sender, RoutedEventArgs e)
        => Value = MathUtilities.Clamp(Value + 1, 0, NumberOfValues - 1);

    public static readonly DirectProperty<HorizontalSelect, IEnumerable<int>> ValuesProperty =
        AvaloniaProperty.RegisterDirect<HorizontalSelect, IEnumerable<int>>(
            nameof(Values),
            o => o.Values,
            (o, v) => o.Values = v,
            defaultBindingMode: BindingMode.TwoWay);

    private IEnumerable<int> _values = Enumerable.Range(0, 1);
    public IEnumerable<int> Values
    {
        get => _values;
        set => SetAndRaise(ValuesProperty, ref _values, value);
    }

    public static readonly DirectProperty<HorizontalSelect, int> NumberOfValuesProperty =
        AvaloniaProperty.RegisterDirect<HorizontalSelect, int>(
            nameof(NumberOfValues),
            o => o.NumberOfValues,
            (o, v) => o.NumberOfValues = v,
            defaultBindingMode: BindingMode.TwoWay);

    private int _numberOfValues;
    public int NumberOfValues
    {
        get => _numberOfValues;
        set => SetAndRaise(NumberOfValuesProperty, ref _numberOfValues, value);
    }

    public static readonly DirectProperty<HorizontalSelect, int> ValueProperty =
        AvaloniaProperty.RegisterDirect<HorizontalSelect, int>(
            nameof(Value),
            o => o.Value,
            (o, v) => o.Value = v,
            defaultBindingMode: BindingMode.TwoWay);

    private int _value;
    public int Value
    {
        get => _value;
        set => SetAndRaise(ValueProperty, ref _value, value); 
    }

    public static readonly DirectProperty<HorizontalSelect, string> TextProperty =
        AvaloniaProperty.RegisterDirect<HorizontalSelect, string>(
            nameof(Text),
            o => o.Text,
            (o, v) => o.Text = v,
            defaultBindingMode: BindingMode.OneWay);

    private string _text = "";
    public string Text
    {
        get => _text;
        set { SetAndRaise(TextProperty, ref _text, value); }
    }
}
