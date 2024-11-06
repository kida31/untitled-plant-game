using System.Collections.Generic;
using System.IO;
using Godot;
using untitledplantgame.Plants;
using PlantData = untitledplantgame.Plants.PlantData;

namespace untitledplantgame.ResourceData.Resources.Plants;

public partial class PlantDatabase : Node
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
	
	public PlantData GetPlantData(int plantId)
	{
		var plantData = _plantDatas.Find(data => data._plantId == plantId);
		if (plantData != null)
			return plantData;

		throw new InvalidDataException($"There was no Data for {plantId}");
	}

	//not sure if we still need this
	public Dictionary<string, Requirement> GetRequirements(int plantId, GrowthStage stage)
	{
		var plantData = _plantDatas.Find(data => data._plantId == plantId);
		if (plantData == null)
		{
			throw new InvalidDataException($"There was no Data for {plantId}");
		}

		var plantRequirements = new Dictionary<string, Requirement>();
		var plantDataRequirementsForStage = plantData.DataForGrowthStages[(int)stage].GrowthRequirements;

		foreach (var data in plantDataRequirementsForStage)
		{
			plantRequirements[data.Name.ToString()] = new Requirement(data.MaxLevel, data.MinLevel);
		}

		return plantRequirements;
	}
}
