namespace untitledplantgame.Plants;

//unsure if i still need this
public interface IPlantable
{
	SoilTile Tile { get; set; }

	public void PlantOnTile(SoilTile soilTile);

	public void AbsorbWaterFromTile();
}
