using Godot;

namespace untitledplantgame.MagicBoxForData;

[GlobalClass]
public partial class RequirementDataForGrowthStage : Resource
{
    [Export] public RequirementData[] GrowthRequirements;
}