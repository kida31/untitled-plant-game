using Godot;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Statistics;

[GlobalClass]
public partial class Modifier : Resource
{
    // Order was chosen to make it easily readable
    [Export(PropertyHint.Enum, "Health,MovementSpeed")]
    public string SelectedOption
    {
        get => _selectedOption;
        set
        {
            _selectedOption = value;
            StatType = CreateStatTypeInstance();
            EmitSignal("changed"); // Notify Godot that the property has changed to update the Inspector
        }
    }
    [Export] public int ModifierValue;
    private string _selectedOption;
    public IStatType StatType { get; private set; }

    public Modifier() : this(0, null)
    {
        ModifierValue = int.MinValue;
    }
    
    public Modifier(int value, IStatType statType)
    {
        ModifierValue = value;
        StatType = statType;
    }
    
    // Technically not a method...
    private IStatType CreateStatTypeInstance() => _selectedOption switch
    {
        "Health" => new Health(),
        "MovementSpeed" => new MovementSpeed(),
        _ => null
    };
}