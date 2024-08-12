namespace untitledplantgame.Plants;

public abstract class SoilTile
{
    private float Hydration
    {
        get;
        set;
    }

    private float Fertilization
    {
        get;
        set;
    }

    void reduceHydration(float reductionValue)
    {
        //minus hydration by value
    }

    void reduceFertilization(float amount)
    {
        
    }
}