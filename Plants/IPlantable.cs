namespace untitledplantgame.Plants;

public interface IPlantable
{
    SoilTile Tile { get; set; }

    public void PlantOnTile(SoilTile soilTile);

    public void AbsorbWaterFromTile();
}