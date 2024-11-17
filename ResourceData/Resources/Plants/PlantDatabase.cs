using System.Collections.Generic;
using System.IO;
using Godot;
using untitledplantgame.Plants;
using PlantData = untitledplantgame.Plants.PlantData;

namespace untitledplantgame.ResourceData.Resources.Plants;

public partial class PlantDatabase : Node, IDatabase<PlantData>
{
	private string _plantDataPath = "res://ResourceData/Resources/Plants";
	private readonly List<PlantData> _plantDatas = new();

	private ResourceManager _resourceManager = ResourceManager.Instance;

	public static PlantDatabase Instance { get; set; }

	public override void _Ready()
	{
		var directories = _resourceManager.LoadDirectoriesFromDirectory(_plantDataPath, new List<string>());
		foreach (var directory in directories)
		{
			_plantDatas.AddRange(_resourceManager.LoadFromDirectory<PlantData>(directory));
		}
	}
	
	private PlantData GetPlantData(int plantId)
	{
		var plantData = _plantDatas.Find(data => data._plantId == plantId);
		if (plantData != null)
			return plantData;

		throw new InvalidDataException($"There was no Data for {plantId}");
	}

	public string DirPath { get; set; } = "res://ResourceData/Resources/Plants";
	public PlantData GetResourceByName(string name)
	{
		return GD.Load<PlantData>("res://ResourceData/Resources/Plants/" + name);
	}

	public PlantData GetResourceById(int id)
	{
		throw new System.NotImplementedException();
	}

	public PlantData[] GetAllResources()
	{
		throw new System.NotImplementedException();
	}
}
