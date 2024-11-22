using Godot;

namespace untitledplantgame.Plants.Models;

[GlobalClass]
public partial class RequirementDataForGrowthStage : Resource
{
	[Export]
	public RequirementData[] GrowthRequirements;

	[Export]
	public int DaysToGrow;

	[Export]
	public bool IsHarvestable;
}
