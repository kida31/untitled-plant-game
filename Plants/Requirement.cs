using System;

namespace untitledplantgame.Plants;

public class Requirement
{
    public Requirement(float maxLevel, float minimumLevel)
    {
        MaxLevel = maxLevel;
        MinimumLevel = minimumLevel;
        CurrentLevel = 0;
    }

    public float MaxLevel { get; private set; }
    public float MinimumLevel { get; private set; }
    public float CurrentLevel { get; set; }

    public bool IsFullfilled()
    {
        return CurrentLevel >= MinimumLevel;
    }

    public override string ToString()
    {
        return string.Format($"The current level reached {CurrentLevel}." +
                             $" The minimum level should be {MinimumLevel}.");
    }
}