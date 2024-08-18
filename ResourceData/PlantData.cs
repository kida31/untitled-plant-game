using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Plants;

namespace untitledplantgame.MagicBoxForData;
[GlobalClass]
public partial class PlantData : Resource
{
    [Export] public int _plantId;
    [Export] private string _plantName;
    [Export] public GrowthStage _growthStage;

    [Export] public RequirementData[] RequirementData;
    
    //public Dictionary<string, RequirementData> Requirements => RequirementData.ToDictionary(x => x.Name, x => x);
}