using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Diagnostics;
using static EstragoniaTemplate.UI.Utilities;

namespace EstragoniaTemplate.UI.Controls;

public partial class SliderFocusser : UserControl
{
    public SliderFocusser()
        => InitializeComponent();

    public static readonly StyledProperty<IInputElement?> SliderProperty =
        AvaloniaProperty.Register<ConfirmableNumericUpDown, IInputElement?>(nameof(Slider));

    public IInputElement? Slider
    {
        get => GetValue(SliderProperty);
        set => SetValue(SliderProperty, value);
    }

    public static readonly DirectProperty<SliderFocusser, string> XYFocusModeProperty =
        AvaloniaProperty.RegisterDirect<SliderFocusser, string>(
            nameof(XYFocusMode),
            o => o.XYFocusMode,
            (o, v) => o.XYFocusMode = v,
            defaultBindingMode: BindingMode.OneWay);

    private string _xyFocusMode = "Disabled";
    public string XYFocusMode
    {
        get => _xyFocusMode;
        set => SetAndRaise(XYFocusModeProperty, ref _xyFocusMode, value);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        XYFocusMode = "Disabled";
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        if (sliderContentControl.Content is Slider slider)
        {
            slider.Focus(NavigationMethodBasedOnMouseOrKey);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled || e is not { Key: Key.Up or Key.Down or Key.Left or Key.Right})
            return;

        XYFocusMode = "Enabled";
    }
}
