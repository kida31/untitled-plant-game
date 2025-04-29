using Godot;

namespace untitledplantgame.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
	[Export] public string PlantName;
	
	[Export] public int ConsumptionRate;
	
	[Export] public int AbsorptionRate;
	
	[Export] public int RootRotThreshold;

	[Export] public RequirementDataForGrowthStage[] DataForGrowthStages;
	
	
}
