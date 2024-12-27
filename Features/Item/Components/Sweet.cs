namespace untitledplantgame.Item.Components;

public partial class Sweet : AComponent
{
	public Sweet() { } //needed to instantiate the class

	public override AComponent Clone() => new Sweet();
}
