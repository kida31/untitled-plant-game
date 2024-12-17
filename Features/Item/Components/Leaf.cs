namespace untitledplantgame.Item.Components;

public partial class Leaf : AComponent
{
	public Leaf() { } //needed to instantiate the class

	public override AComponent Clone() => new Leaf();
}
