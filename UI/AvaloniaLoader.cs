using Avalonia;
using Godot;
using JLeb.Estragonia;
using System;

namespace EstragoniaTemplate.UI;

public partial class AvaloniaLoader : Node
{
    public static AvaloniaLoader Instance { get; set; } = null!;
    public event EventHandler<double>? UIScaleChanged;
    public double UIScaling { get; private set; } = 1;
    private double _pendingUIScaling = 1;

    private float _resolutionTargetWidth = 960;
    private float _resolutionTargetHeight = 540;
    private double _resizeGracePeriod = 0.1;
    private double _elapsedSinceLastResize = 0;

    private double ComputeUIScale(Window window)
    {
        var xRatio = window.Size.X / _resolutionTargetWidth;
        var yRatio = window.Size.Y / _resolutionTargetHeight;

        return Mathf.Min(xRatio, yRatio);
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
            UIScaleChanged?.Invoke(this, UIScaling);
        }
    }
}