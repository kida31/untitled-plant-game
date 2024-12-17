namespace untitledplantgame.Item.Components;

public partial class DriedLeaf : AComponent
{
	public DriedLeaf() { } //needed to instantiate the class

	public override AComponent Clone() => new DriedLeaf();
}
