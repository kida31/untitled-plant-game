namespace untitledplantgame.Plants;

public class Requirement
{
    public Requirement(float maxLevel, float minimumLevel, float currentLevel)
    {
        this.maxLevel = maxLevel;
        this.minimumLevel = minimumLevel;
        this.currentLevel = currentLevel;
    }
    private float maxLevel;
    private float minimumLevel;
    
    public float currentLevel { get; set; }

    public bool isFullfilled()
    {
        return currentLevel >= minimumLevel;
    }
}