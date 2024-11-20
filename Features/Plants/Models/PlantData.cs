using Godot;

namespace untitledplantgame.Plants.Models;

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
