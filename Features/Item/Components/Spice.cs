namespace untitledplantgame.Item.Components;

public partial class Spice : AComponent
{
	public Spice() {} //needed to instantiate the class
	public override AComponent Clone() => new Spice();
}
