using Godot;

namespace untitledplantgame.Plants;

[GlobalClass]
public partial class PlantDemand : Resource
{
	[Export] public RequirementType Type { get; private set; }
	[Export] public float ConsumptionRate { get; private set; }
	
	[Export] public float AbsorptionRate { get; private set; }
}
