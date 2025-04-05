using Godot;
using Godot.Collections;

namespace EstragoniaTemplate.Resources;

public partial class InputMapResource : Resource
{
    [Export]
    public Dictionary<StringName, Array<InputEvent>> InputDictionary { get; set; } = new();

    public const string InputMapResourcePath = "user://input_map.tres";

    public static void SaveCurrentInputMap()
    {
        var inputMapResource = new InputMapResource();
        foreach (var action in InputMap.GetActions())
        {
            inputMapResource.InputDictionary.Add(action, InputMap.ActionGetEvents(action));
        }
        ResourceSaver.Save(inputMapResource, InputMapResourcePath);
    }

    /// <summary>
    /// Does nothing if there is no saved input map yet.
    /// </summary>
    public static void LoadSavedInputMap()
    {
        if (!FileAccess.FileExists(InputMapResourcePath))
            return;

        var inputMapResource = (InputMapResource)ResourceLoader.Load(InputMapResourcePath);
        foreach (var (action, inputEvents) in inputMapResource.InputDictionary)
        {
            InputMap.ActionEraseEvents(action);
            foreach (var inputEvent in inputEvents)
            {
                InputMap.ActionAddEvent(action, inputEvent);
            }
        }
    }
}
