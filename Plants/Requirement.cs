namespace untitledplantgame.Plants;

public class Requirement
{
	public Requirement(float maxLevel, float minimumLevel)
	{
		MaxLevel = maxLevel;
		MinimumLevel = minimumLevel;
		CurrentLevel = 0;
	}

	public float MaxLevel { get; }
	private float MinimumLevel { get; }
	public float CurrentLevel { get; set; }

	public bool IsFulfilled()
	{
		return CurrentLevel >= MinimumLevel;
	}

	public override string ToString()
	{
		return string.Format($"The current level reached {CurrentLevel}, minimum level: {MinimumLevel}, maximum level: {MaxLevel}");
	}
}
