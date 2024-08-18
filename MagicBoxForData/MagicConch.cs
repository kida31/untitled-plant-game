using System.Collections.Generic;
using Godot;
using untitledplantgame.Plants;

public partial class MagicConch
{
    public static MagicConch Instance = new MagicConch();

    public Dictionary<string,Requirement> GetRequirements(int plantId, GrowthStage stage)
    {
        //get specific Requirements based on id and stage

        Requirement sunRequirement = MakeSomeRandomRequirement();

        Dictionary<string, Requirement> requirements = new Dictionary<string, Requirement>();
        requirements["sun"] = sunRequirement;
        return requirements;
    }

    Requirement MakeSomeRandomRequirement()
    {
        return new Requirement(100, 100, 0);
    }
}
