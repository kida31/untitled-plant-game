using System;

namespace untitledplantgame.Plants;

public class Requirement
{
    public Requirement(float maxLevel, float minimumLevel, float currentLevel, int daysToGrow, int currentGrowthDay)
    {
        MaxLevel = maxLevel;
        MinimumLevel = minimumLevel;
        CurrentLevel = currentLevel;
        DaysToGrow = daysToGrow;
        CurrentGrowthDay = currentGrowthDay;
    }

    public float MaxLevel { get; private set; }
    public float MinimumLevel { get; private set; }
    public float CurrentLevel { get; set; }
    private int DaysToGrow { get; set; }
    public int CurrentGrowthDay { get; set; }

    void CheckCurrentRequirement()
    {
        if (CurrentLevel >= MinimumLevel)
            CurrentGrowthDay++;
    }

    public bool CanAdvanceStage()
    {
        CheckCurrentRequirement();
        return CurrentGrowthDay >= DaysToGrow;
    }

    public override string ToString()
    {
        return string.Format($"The current level reached {CurrentLevel}." +
                             $" The minimum level should be {MinimumLevel}.");
    }
}