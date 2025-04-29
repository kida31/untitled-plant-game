using Godot;

namespace untitledplantgame.Plants;

[GlobalClass]
public partial class Requirement : Resource
{
	[Export] public RequirementType Type { get; set; }
	[Export] public float MaxLevel { get; set; }
	[Export] private float MinimumLevelToGrow { get; set; }
	[Export] public float CurrentLevel { get; set; }

	public Requirement()
	{
	}

	public Requirement(RequirementType type, float maxLevel, float minimumLevelToGrow)
	{
		Type = type;
		MaxLevel = maxLevel;
		MinimumLevelToGrow = minimumLevelToGrow;
		CurrentLevel = 0;
	}

	public bool IsFulfilled()
	{
		return CurrentLevel >= MinimumLevelToGrow;
	}

	public override string ToString()
	{
		return string.Format($"The current level reached {CurrentLevel}, minimum level: {MinimumLevelToGrow}, maximum level: {MaxLevel}");
	}
}
