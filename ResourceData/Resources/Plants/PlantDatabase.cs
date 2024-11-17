using System.Linq;
using Godot;
using untitledplantgame.Plants;

namespace untitledplantgame.ResourceData.Resources.Plants;

public partial class PlantDatabase : Node, IDatabase<PlantData>
{
	public static PlantDatabase Instance { get; private set; }

	public string DirPath
	{
		get => _dirPath;
		set => _dirPath = value;
	}
	
	private string _dirPath = "res://ResourceData/Resources/Plants";

	public override void _Ready()
	{
		Instance = this;
	}

	public PlantData GetResourceByName(string name)
	{
		// name == "Basil"
		// turns into
		// "res://ResourceData/Resources/Plants/Basil/Basil.tres"
		return GD.Load<PlantData>($"{_dirPath}/{name}/{name}.tres");
	}

	public PlantData GetResourceById(int id)
	{
		return GetAllResources().FirstOrDefault(plant => plant._plantId == id, null);
	}

	public PlantData[] GetAllResources()
	{
		// Check all directories in the Plants directory
		// Directory names are the names of the plants
		// Load the PlantData from the .tres file in each directory
		var subDirectories = DirAccess.GetDirectoriesAt(_dirPath);
		return subDirectories.Select(GetResourceByName).ToArray();
	}
}
