using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using untitledplantgame.Plants;

namespace untitledplantgame.MagicBoxForData;

public partial class ResourceManager : Node
{
    public static ResourceManager Instance { get; private set; }

    private string _plantDataPath = "res://ResourceData/Resources";
    private List<PlantData> _plantDatas = new ();

    public override void _Ready()
    {
        var directories = LoadDirectoriesFromDirectory(_plantDataPath, new List<string>());
        foreach (var directory in directories)
        {
            GD.Print("Loading from directory: " + directory);
            _plantDatas.AddRange(LoadFromDirectory<PlantData>(directory));
        }
        
        Instance ??= this;
    }

    private T[] LoadFromDirectory<T>(string dirPath) where T : Resource
    {
        var fileNames = DirAccess.GetFilesAt(dirPath);

        return fileNames.Select(fileName => dirPath + "/" + fileName)
            .Select(filePath =>
            {
                GD.Print(filePath);
                return GD.Load(filePath) as T;
            })
            .ToArray();
    }

    private string[] LoadDirectoriesFromDirectory(string dirPath, List<string> results)
    {
        var dirNames = DirAccess.GetDirectoriesAt(dirPath);
        results.AddRange(dirNames.Select(d => dirPath + "/" + d));

        if (dirNames.Length <= 0) return results.ToArray();

        foreach (var dirName in dirNames)
        {
            LoadDirectoriesFromDirectory(dirPath + "/" + dirName, results);
        }

        return results.ToArray();
    }

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
            plantRequirements[data.Name] = new Requirement(data.MaxLevel, data.MinLevel, data.CurrentLevel);
        }

        return plantRequirements;
    }
}