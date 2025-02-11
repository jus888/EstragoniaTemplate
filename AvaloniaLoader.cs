using Avalonia;
using Godot;
using JLeb.Estragonia;

namespace EstragoniaUI;

public partial class AvaloniaLoader : Node
{

    public override void _Ready()
        => AppBuilder
            .Configure<App>()
            .UseGodot()
            .SetupWithoutStarting();

}