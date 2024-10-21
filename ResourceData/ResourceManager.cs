using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;

namespace untitledplantgame.MagicBoxForData;

[Singleton]
public partial class ResourceManager : Node
{
	public static ResourceManager Instance { get; private set; }

	private string _plantDataPath = "res://ResourceData/Resources";
	private readonly List<PlantData> _plantDatas = new();
	private readonly Logger _logger = new("ResourceManager");

	public override void _Ready()
	{
		var directories = LoadDirectoriesFromDirectory(_plantDataPath, new List<string>());
		foreach (var directory in directories)
		{
			_logger.Info("Loading from directory: " + directory);
			_plantDatas.AddRange(LoadFromDirectory<PlantData>(directory));
		}

		Instance ??= this;
	}

	private T[] LoadFromDirectory<T>(string dirPath)
		where T : Resource
	{
		var fileNames = DirAccess.GetFilesAt(dirPath);

		return fileNames
			.Select(fileName => dirPath + "/" + fileName)
			.Select(filePath =>
			{
				_logger.Info(filePath);
				return GD.Load(filePath) as T;
			})
			.ToArray();
	}

	private string[] LoadDirectoriesFromDirectory(string dirPath, List<string> results)
	{
		var dirNames = DirAccess.GetDirectoriesAt(dirPath);
		results.AddRange(dirNames.Select(d => dirPath + "/" + d));

		if (dirNames.Length <= 0)
			return results.ToArray();

		foreach (var dirName in dirNames)
		{
			LoadDirectoriesFromDirectory(dirPath + "/" + dirName, results);
		}

		return results.ToArray();
	}

	public PlantData GetPlantData(int plantId)
	{
		var plantData = _plantDatas.Find(data => data._plantId == plantId);
		if (plantData != null)
			return plantData;

		_logger.Error($"There was no Data for {plantId}");
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
		var plantDataRequirementsForStage = plantData
			.DataForGrowthStages[(int)stage]
			.GrowthRequirements;

		foreach (var data in plantDataRequirementsForStage)
		{
			plantRequirements[data.Name.ToString()] = new Requirement(data.MaxLevel, data.MinLevel);
		}

		return plantRequirements;
	}
}
