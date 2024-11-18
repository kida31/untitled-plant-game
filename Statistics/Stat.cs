using System;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Statistics;

[GlobalClass]
public partial class Stat : Resource
{
	// Order was chosen to make it easily readable
	[Export(PropertyHint.Enum, "Health,MovementSpeed,Faith,Currency")]
	private string SelectedOption
	{
		get => _selectedOption;
		set
		{
			_selectedOption = value;
			StatType = CreateInstance();
		}
	}

	[Export]
	public int StatBaseValue;

	[Export]
	public bool IsHidden;

	[Export]
	public Array<int> StatModifiers { get; set; } = new();
	private string _selectedOption;
	public IStatType StatType { get; set; }

	// Make sure you provide a parameterless constructor.
	// In C#, a parameterless constructor is different from a
	// constructor with all default values.
	// Without a parameterless constructor, Godot will have problems
	// creating and editing your resource via the inspector.
	public Stat()
		: this(0, null, false)
	{
		StatBaseValue = int.MinValue;
	}

	public Stat(int baseValue, IStatType statType, bool isHidden)
	{
		StatBaseValue = baseValue;
		StatType = statType;
		IsHidden = isHidden;
	}

	/**
	* Strings must match the ones from the dropdown menu/PropertyHint
	*/
	private IStatType CreateInstance() =>
		_selectedOption switch
		{
			"Health" => new Health(),
			"MovementSpeed" => new MovementSpeed(),
			_ => null,
		};

	public void AssignBaseValueOfStat(int baseValue)
	{
		if (StatBaseValue != int.MinValue)
		{
			throw new InvalidOperationException("The variable of type: " + this + " already has a BaseStatValue");
		}

		StatBaseValue = baseValue;
	}

	public int GetBaseValueOfStat()
	{
		if (StatBaseValue == int.MinValue)
		{
			throw new InvalidOperationException("BaseStatValue is not assigned!");
		}

		return StatBaseValue;
	}

	public void AddStatModifier(int modifier)
	{
		StatModifiers.Add(modifier);
	}

	public int GetModifiedStatValue()
	{
		// Base stat + sum of all modifiers
		return StatBaseValue + StatModifiers.Sum();
	}

	public void AddMultipleModifiers(Array<int> modifiers)
	{
		StatModifiers.AddRange(modifiers);
	}

	public Array<int> GetStatModifiers()
	{
		return StatModifiers;
	}

	// Technically not a method...
	public IStatType CreateStatTypeInstance(IStatType inputStatType) =>
		inputStatType switch
		{
			Health => new Health(),
			MovementSpeed => new MovementSpeed(),
			_ => null,
		};
}
