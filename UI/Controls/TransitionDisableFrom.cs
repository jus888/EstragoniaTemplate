using Avalonia;
using Avalonia.Animation;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using Avalonia.VisualTree;
using EstragoniaTemplate.UI.Views;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI.Controls;

public class TransitionDisableFrom : IPageTransition
{
    public TimeSpan Duration { get; set; }

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        if (from is not InputElement fromElement || to is not InputElement toElement)
        {
            return;
        }

        toElement.IsEnabled = true;
        if (cancellationToken.IsCancellationRequested)
        {
            fromElement.IsEnabled = true;
            return;
        }
        fromElement.IsEnabled = false;

        await Task.Delay(Duration, CancellationToken.None);

        fromElement.IsEnabled = true;
    }
}
