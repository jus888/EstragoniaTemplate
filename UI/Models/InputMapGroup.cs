using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI.Models;

public class InputMapGroup
{
    public HashSet<Key> ReservedKeys { get; private init; } = new();
    public Dictionary<Key, InputMapItem> KeyMappings { get; } = new();
    public Dictionary<JoyButton, InputMapItem> JoypadMappings { get; } = new();

    public InputMapGroup(HashSet<Key> reservedKeys)
    {
        ReservedKeys = reservedKeys;
    }
}
