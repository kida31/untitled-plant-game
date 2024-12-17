namespace untitledplantgame.Item.Components;

public partial class Nuke : AComponent
{
	public Nuke() { } //needed to instantiate the class

	public override AComponent Clone() => new Nuke();
}
