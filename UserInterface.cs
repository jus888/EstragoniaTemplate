using Godot;
using System;
using EstragoniaTemplate.UI;
using JLeb.Estragonia;

namespace EstragoniaTemplate;

public partial class UserInterface : AvaloniaControl
{
    public override void _Ready()
    {
        RenderScaling = AvaloniaLoader.Instance.UIScaling;
        AvaloniaLoader.Instance.UIScaleChanged += (_, scale) => 
        { 
            RenderScaling = scale; 
        };

        Control = new BaseView();

        base._Ready();
    }
}
