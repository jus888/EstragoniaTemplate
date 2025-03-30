using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace EstragoniaTemplate.UI.Views;

public partial class OptionsControlsView : View
{
    private const int focusInflation = 30;

    public OptionsControlsView()
    {
        InitializeComponent();

        scrollViewer.BringIntoViewOnFocusChange = false;
        scrollViewer.GotFocus += (s, e) =>
        {
            var control = e.Source as Control;
            var inflatedSize = control?.DesiredSize.Inflate(new(focusInflation)) ?? default;
            control?.BringIntoView(new Rect(-new Point(focusInflation, focusInflation), inflatedSize));
        };
    }
}
