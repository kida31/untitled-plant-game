namespace untitledplantgame.Item.Components;

public partial class Sunflower : AComponent
{
	public Sunflower() {} //needed to instantiate the class

	public override AComponent Clone() => new Sunflower();
}
