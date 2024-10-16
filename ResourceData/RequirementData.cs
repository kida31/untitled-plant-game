using Godot;

namespace untitledplantgame.MagicBoxForData;

[GlobalClass]
public partial class RequirementData : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public float CurrentLevel { get; private set; }
    [Export] public float MinLevel { get; private set; }
    [Export] public float MaxLevel { get; private set; }
    [Export] public int DaysToGrow { get; set; }
    [Export] public int CurrentGrowthDay { get; set; }
}