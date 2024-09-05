using System;

namespace untitledplantgame.Plants;

public class Requirement
{
    public Requirement(float maxLevel, float minimumLevel, float currentLevel)
    {
        MaxLevel = maxLevel;
        MinimumLevel = minimumLevel;
        CurrentLevel = currentLevel;
    }

    public float MaxLevel { get; private set; }
    public float MinimumLevel { get; private set; }

    public float CurrentLevel { get; set; }

    public bool IsFulfilled()
    {
        return CurrentLevel >= MinimumLevel;
    }

    public override string ToString()
    {
        return string.Format($"The current level reached {CurrentLevel}." +
                             $" The minimum level should be {MinimumLevel} to advance");
    }
}