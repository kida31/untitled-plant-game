using System.Linq;
using Godot;
using Godot.Collections;

namespace untitledplantgame.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
	[Export] public string PlantName;
	
	[Export] public Array<PlantDemand> PlantDemands;
	
	[Export] public int MaxRootHealth;

	[Export] public RequirementDataForGrowthStage[] DataForGrowthStages;
	
	public PlantDemand GetDemand(RequirementType requirementType) => PlantDemands.FirstOrDefault(demand => demand.Type == requirementType);
}
