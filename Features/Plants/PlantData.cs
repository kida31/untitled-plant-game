using Godot;
using RequirementDataForGrowthStage = untitledplantgame.Plants.Models.RequirementDataForGrowthStage;

namespace untitledplantgame.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
	[Export]
	public int _plantId;

	[Export]
	public string _plantName;

	[Export]
	public RequirementDataForGrowthStage[] DataForGrowthStages;
}
