using Godot;

namespace untitledplantgame.MagicBoxForData;

[GlobalClass]
public partial class PlantData : Resource
{
    [Export] public int _plantId;
    [Export] private string _plantName;

    [Export] public RequirementDataForGrowthStage[] DataForGrowthStages;
}