using Godot;

namespace untitledplantgame.Plants.Models;

[GlobalClass]
public partial class PlantData : Resource
{
	[Export] public string PlantName;

	[Export] public RequirementDataForGrowthStage[] DataForGrowthStages;
	
	[Export] public SpriteFrames Sprite;
}
