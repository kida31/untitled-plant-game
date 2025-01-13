using Godot;
using untitledplantgame.Plants.Models;

namespace untitledplantgame.Item.Components;

[GlobalClass]
public partial class SeedComponent : AComponent
{
	//which option?
	[Export] public string PlantName { get; set; }
	[Export] private PlantData _plantData;

	public SeedComponent()
	{
	}

	public SeedComponent(string plantName)
	{
		PlantName = plantName;
	}

	public override AComponent Clone()
	{
		throw new System.NotImplementedException();
	}
}
