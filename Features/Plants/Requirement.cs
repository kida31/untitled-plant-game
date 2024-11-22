namespace untitledplantgame.Plants;

public class Requirement
{
	public Requirement(float maxLevel, float minimumLevelToGrow)
	{
		MaxLevel = maxLevel;
		MinimumLevelToGrow = minimumLevelToGrow;
		CurrentLevel = 0;
	}

	public float MaxLevel { get; }
	public float CurrentLevel { get; set; }
	private float MinimumLevelToGrow { get; }

	public bool IsFulfilled()
	{
		return CurrentLevel >= MinimumLevelToGrow;
	}

	public override string ToString()
	{
		return string.Format($"The current level reached {CurrentLevel}, minimum level: {MinimumLevelToGrow}, maximum level: {MaxLevel}");
	}
}
