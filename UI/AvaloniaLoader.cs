using Avalonia;
using Godot;
using JLeb.Estragonia;
using System;

namespace EstragoniaTemplate.UI;

public partial class AvaloniaLoader : Node
{
    public static bool LastPressedInputWasMouseClick = true;

    public static AvaloniaLoader Instance { get; set; } = null!;
    public event EventHandler<double>? UIScaleChanged;

    private double _uiScalingOption = 1;
    public double UIScalingOption
    {
        get => _uiScalingOption;
        set
        {
            _uiScalingOption = value;
            _pendingUIScaling = ComputeUIScale(GetWindow());
            _elapsedSinceLastResize = _resizeGracePeriod;
        }
    }

    private double _uiScaling;
    public double UIScaling
    {
        get => _uiScaling;
        private set
        {
            _uiScaling = value;
            UIScaleChanged?.Invoke(this, _uiScaling);
        }
    }
    private double _pendingUIScaling = 1;

    private float _resolutionTargetWidth = 960;
    private float _resolutionTargetHeight = 540;
    private double _resizeGracePeriod = 0.1;
    private double _elapsedSinceLastResize = 0;

    private double ComputeUIScale(Window window)
    {
        var xRatio = window.Size.X / _resolutionTargetWidth;
        var yRatio = window.Size.Y / _resolutionTargetHeight;

        return Mathf.Min(xRatio, yRatio) * UIScalingOption;
    }

    public override void _Ready()
    {
        AppBuilder
            .Configure<App>()
            .UseGodot()
            .SetupWithoutStarting();

        var window = GetWindow();
        window.SizeChanged += () =>
        {
            _pendingUIScaling = ComputeUIScale(window);
            _elapsedSinceLastResize = 0;
        };

        UIScaling = ComputeUIScale(window);
        _pendingUIScaling = UIScaling;

        Instance = this;
    }

    public override void _Process(double delta)
    {
        _elapsedSinceLastResize += delta;

        if (UIScaling != _pendingUIScaling && _elapsedSinceLastResize > _resizeGracePeriod)
        {
            UIScaling = _pendingUIScaling;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
        {
            LastPressedInputWasMouseClick = true;
        }
        else if ((@event is InputEventKey key && key.Pressed) || (@event is InputEventJoypadButton joypadButton && joypadButton.Pressed))
        {
            LastPressedInputWasMouseClick = false;
        }
    }
}