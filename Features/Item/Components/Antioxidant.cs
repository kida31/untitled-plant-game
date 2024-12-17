namespace untitledplantgame.Item.Components;

public partial class Antioxidant : AComponent
{
	public Antioxidant() { } //needed to instantiate the class

	public override AComponent Clone() => new Antioxidant();
}
