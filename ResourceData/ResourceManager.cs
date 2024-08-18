
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using untitledplantgame.Plants;

namespace untitledplantgame.MagicBoxForData;

public partial class ResourceManager : Node
{
    public static ResourceManager Instance;

    private string _plantDataPath = "res://ResourceData/Resources/";
    private PlantData[] _plantDatas;

    public override void _Ready()
    {
        _plantDatas = LoadFromDirectory<PlantData>(_plantDataPath);
        if (Instance == null)
        {
            Instance = this;
        }

        Instance.GetRequirements(0, GrowthStage.Flowering);
        GD.Print(":C");
    }

    private T[] LoadFromDirectory<T>(string dirPath) where T : Resource
    {
        var fileNames = DirAccess.GetFilesAt(dirPath);

        return fileNames.Select(fileName => dirPath + fileName)
            .Select(filePath => GD.Load(filePath) as T)
            .ToArray();
    }

    public Dictionary<string, Requirement> GetRequirements(int plantId, GrowthStage stage)
    {
        var plantData = Array.Find(_plantDatas, data => data._plantId == plantId && data._growthStage == stage);
        if (plantData == null)
        {
            throw new InvalidDataException($"There was no Data for {plantId} with stage {stage}");
        }
        
        var plantRequirements = new Dictionary<string, Requirement>();
        
        foreach (var data in plantData.RequirementData)
        {
            plantRequirements[data.Name] = new Requirement(data.MaxLevel, data.MinLevel, data.CurrentLevel);
        }
        
        return plantRequirements;
    }

    Requirement MakeSomeRandomRequirement()
    {
        return new Requirement(100, 100, 0);
    }
}