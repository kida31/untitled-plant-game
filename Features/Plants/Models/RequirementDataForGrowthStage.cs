using Godot;

namespace untitledplantgame.Plants;

[GlobalClass]
public partial class RequirementDataForGrowthStage : Resource
{
	[Export] public Requirement[] GrowthRequirements;

	[Export] public bool IsHarvestable;
}
