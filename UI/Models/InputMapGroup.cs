using Godot;
using System.Collections.Generic;

namespace EstragoniaTemplate.UI.Models;

public class InputMapGroup
{
    public HashSet<Key> ReservedKeys { get; private init; } = new();
    public Dictionary<Key, InputMapItem> KeyMappings { get; } = new();
    public Dictionary<JoyButton, InputMapItem> JoypadMappings { get; } = new();

    public InputMapGroup(HashSet<Key>? reservedKeys = null)
    {
        ReservedKeys = reservedKeys ?? new();
    }
}
