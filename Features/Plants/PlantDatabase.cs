using System.IO;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants.Models;

namespace untitledplantgame.Plants;

/// <summary>
/// A database singleton for all plant resources.
/// </summary>
public partial class PlantDatabase : Node, IDatabase<PlantData>
{
	public static PlantDatabase Instance { get; private set; }

	public string DirPath => _dirPath;

	private Logger _logger;
	private string _dirPath = "res://Resources/Plants/";

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
		var plantName = name.Replace(" ", "_");
		var resourceName = $"{plantName}.tres";
		var path = Path.Join(_dirPath, resourceName);
		var data = GD.Load<PlantData>(path);
		_logger.Debug($"Loaded PlantData resource from path {path}");;
		if (data == null)
		{
			_logger.Error("There is no PlantData resource with the name " + name);
		}
		return data;
	}

	public PlantData[] GetAllResources()
	{
		// Check all directories in the Plants directory.
		// Directory names are the names of the plants
		// Load the PlantData from the .tres file in each directory
		var subDirectories = DirAccess.GetDirectoriesAt(_dirPath);
		return subDirectories.Select(GetResourceByName).ToArray();
	}
}
