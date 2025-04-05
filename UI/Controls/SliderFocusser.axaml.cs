using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

        if (e.Handled || e is not { KeyModifiers: KeyModifiers.None, Key: Key.Down or Key.Up })
            return;

        IInputElement? nextFocus = null;
        switch (e.Key)
        {
            case Key.Up:
                nextFocus = KeyboardNavigationHandler.GetNext(this, NavigationDirection.Up);
                break;
            case Key.Down:
                nextFocus = KeyboardNavigationHandler.GetNext(this, NavigationDirection.Down);
                break;
            case Key.Left:
                nextFocus = KeyboardNavigationHandler.GetNext(this, NavigationDirection.Left);
                break;
            case Key.Right:
                nextFocus = KeyboardNavigationHandler.GetNext(this, NavigationDirection.Right);
                break;
        }

        if (nextFocus != null && nextFocus.Focusable)
        {
            nextFocus.Focus(NavigationMethodBasedOnMouseOrKey);
        }
    }
}
