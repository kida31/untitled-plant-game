namespace untitledplantgame.Item.Components;

public partial class PlantComponent : AComponent
{
	public MedicinalComponent MedicinalComponent;
	public IllnessComponent IllnessComponent;

	public PlantComponent()
	{
	}

	public PlantComponent(MedicinalComponent medicine, IllnessComponent illness)
	{
		MedicinalComponent = medicine;
		IllnessComponent = illness;
	}

	public override IllnessComponent Clone()
	{
		throw new System.NotImplementedException();
	}
}
