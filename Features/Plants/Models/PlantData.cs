using Godot;

namespace untitledplantgame.Plants.Models;

[GlobalClass]
public partial class PlantData : Resource
{
	[Export] public string PlantName;
	
	[Export] public int ConsumptionRate;
	
	[Export] public int AbsorptionRate;

	[Export] public RequirementDataForGrowthStage[] DataForGrowthStages;
	
	
}
