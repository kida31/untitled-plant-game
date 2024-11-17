using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;

namespace untitledplantgame.ResourceData;


/// <summary>
/// A database singleton for all plant resources.
/// </summary>
public partial class PlantDatabase : Node, IDatabase<PlantData>
{
	public static PlantDatabase Instance { get; private set; }

	public string DirPath
	{
		get => _dirPath;
		set => _dirPath = value;
	}

	private Logger _logger;
	private string _dirPath = "res://ResourceData/Resources/Plants";

	public override void _Ready()
	{
		_logger = new Logger(this);
		
		if (Instance != null)
		{
			_logger.Error("There are multiple PlantDatabases in the scene. There should only be one.");
			QueueFree();
			return;
		}
		
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
