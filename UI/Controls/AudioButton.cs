using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI.Controls;

public class AudioButton : Button
{
    public static readonly StyledProperty<string> ClickSoundProperty =
        AvaloniaProperty.Register<AudioButton, string>(nameof(ClickSound), defaultValue: string.Empty);

    public string ClickSound
    {
        get => GetValue(ClickSoundProperty);
        set => SetValue(ClickSoundProperty, value);
    }

    protected override void OnClick()
    {
        if (!string.IsNullOrEmpty(ClickSound) && Enum.TryParse<AudioManager.Sound>(ClickSound, out var sound))
        {
            AudioManager.Instance?.Play(sound, AudioManager.Bus.UI);
        }

        base.OnClick();
    }
}
