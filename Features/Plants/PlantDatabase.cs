using System;
using System.IO;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants.Models;
using untitledplantgame.ResourceData;

namespace untitledplantgame.Plants;

/// <summary>
/// A database singleton for all plant resources.
/// </summary>
public partial class PlantDatabase : Node
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
		var pathName = $"{name}.tres";
		return GD.Load<PlantData>(Path.Join(_dirPath, pathName));
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
