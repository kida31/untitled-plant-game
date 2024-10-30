using Godot;
using Godot.Collections;
using untitledplantgame.Statistics;

namespace untitledplantgame.Entity;

[GlobalClass]
public partial class EntityConfiguration : Resource
{
    [Export] public string Notes { get; set; }
    [Export] public Array<Stat> Stats { get; set; } = new ();
    
    
    // Make sure you provide a parameterless constructor.
    // In C#, a parameterless constructor is different from a
    // constructor with all default values.
    // Without a parameterless constructor, Godot will have problems
    // creating and editing your resource via the inspector.
    public EntityConfiguration() : this(null)
    {
    }

    public EntityConfiguration(string note)
    {
        Notes = note;
    }

    public void PrintOutAllModifiedStats()
    {
        foreach (var aStat in Stats)
        {
            GD.Print(aStat.GetModifiedStatValue());
        }
    }
}