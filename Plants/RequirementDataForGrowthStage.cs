using Godot;

namespace untitledplantgame.Plants;

[GlobalClass]
public partial class RequirementDataForGrowthStage : Resource
{
	[Export]
	public Plants.RequirementData[] GrowthRequirements;

	[Export]
	public int DaysToGrow;
}
