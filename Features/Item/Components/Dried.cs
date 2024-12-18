namespace untitledplantgame.Item.Components;

public partial class Dried : AComponent
{
	public Dried() { } //needed to instantiate the class

	public override AComponent Clone() => new Dried();
}
