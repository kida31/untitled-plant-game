namespace untitledplantgame.Plants;

public interface IPlantable
{
    SoilTile tile
    {
        get;
        set;
    }
    
    //designate tile it's on
    void PlantOnTile();

    //hydrates based on how much water is on soil tile
    void Hydrate();
}