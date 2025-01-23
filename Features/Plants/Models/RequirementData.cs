using Godot;

namespace untitledplantgame.Plants.Models;

[GlobalClass]
public partial class RequirementData : Resource
{
	[Export] public RequirementType Name { get; private set; }

	[Export] public float MinLevel { get; private set; }

	[Export] public float MaxLevel { get; private set; }
}
