namespace untitledplantgame.Item.Components;

public partial class Rose : AComponent
{
	public Rose() {} //needed to instantiate the class
	public override AComponent Clone() => new Rose();
}
