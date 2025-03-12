using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI.Controls;

public class FocusableSlider : Slider
{
    protected override Type StyleKeyOverride => typeof(Slider);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e is { KeyModifiers: KeyModifiers.None, Key: Key.Up or Key.Down })
            return;

        base.OnKeyDown(e);
    }
}
