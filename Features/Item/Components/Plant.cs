namespace untitledplantgame.Item.Components;

public partial class Plant : AComponent
{
	public MedicinalComponent MedicinalComponent;
	public IllnessComponent IllnessComponent;

	public Plant()
	{
	}

	public Plant(MedicinalComponent medicine, IllnessComponent illness)
	{
		MedicinalComponent = medicine;
		IllnessComponent = illness;
	}

	public override AComponent Clone()
	{
		throw new System.NotImplementedException();
	}
}
