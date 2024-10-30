using System;
using Godot;
using Godot.Collections;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Statistics;

[GlobalClass]
public partial class Stat : Resource
{
    // Order was chosen to make it easily readable
    [Export(PropertyHint.Enum, "Health,MovementSpeed")]
    private string SelectedOption
    {
        get => _selectedOption;
        set
        {
            _selectedOption = value;
            StatType = CreateInstance();
            EmitSignal("changed"); // Notify Godot that the property has changed to update the Inspector
        }
    }
    [Export] public int StatValue;
    [Export] public Array<Modifier> StatModifiers { get; set; } = new ();
    private string _selectedOption;
    public IStatType StatType { get; set; }
    
    // Make sure you provide a parameterless constructor.
    // In C#, a parameterless constructor is different from a
    // constructor with all default values.
    // Without a parameterless constructor, Godot will have problems
    // creating and editing your resource via the inspector.
    public Stat() : this(0, null)
    {
        StatValue = int.MinValue;
    }

    public Stat(int value, IStatType statType)
    {
        StatValue = value;
        StatType = statType;
    }
    
    /**
    * Strings must match the ones from the dropdown menu/PropertyHint
    */
    private IStatType CreateInstance() => _selectedOption switch
    {
        "Health" => new Health(),
        "MovementSpeed" => new MovementSpeed(),
        _ => null
    };

    public void AssignBaseValueOfStat(int baseValue)
    {
        if (StatValue != int.MinValue)
        {
            throw new InvalidOperationException("The variable of type: " + this + " already has a BaseStatValue");
        }
        
        StatValue = baseValue;
    }
    
    public int GetBaseValueOfStat()
    {
        if (StatValue == int.MinValue)
        {
            throw new InvalidOperationException("BaseStatValue is not assigned!");
        }

        return StatValue;
    }
    
    public void AddStatModifier(Modifier modifier)
    {
        if (modifier.StatType.GetType() == this.StatType.GetType())
        {
            StatModifiers.Add(modifier);
        }
        else
        {
            throw new InvalidOperationException(
                "Tried to add a modifier to " + this.StatType + " with the incompatible type: " + modifier.StatType);
        }
    }
    
    public int GetModifiedStatValue()
    {
        int finalValue = 0;
        foreach (var mod in StatModifiers)
        {
            if (mod.StatType.GetType() == this.StatType.GetType())
            {
                finalValue += mod.ModifierValue;
            }
            else
            {
                throw new InvalidOperationException(
                    "Type mismatch! Can't add Modifier Type: " + mod.StatType + " to Stat of Type " + this.StatType);
            }
        }
        finalValue += StatValue;
        return finalValue;
    }

    public void AddMultipleModifiers(Array<Modifier> modifiers)
    {
        foreach (var modifier in modifiers)
        {
            StatModifiers.Add(new Modifier(modifier.ModifierValue, CreateStatTypeInstance(modifier.StatType)));
        }
    }

    public Array<Modifier> GetStatModifiers()
    {
        return StatModifiers;
    }

    // Technically not a method...
    public IStatType CreateStatTypeInstance(IStatType inputStatType) => inputStatType switch
    {
        Health => new Health(),
        MovementSpeed => new MovementSpeed(),
        _ => null
    };
}