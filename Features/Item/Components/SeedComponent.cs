using untitledplantgame.Item;

namespace untitledplantgame.Database;

public partial class SeedComponent : AComponent
{
	public string PlantName { get; set; }

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
