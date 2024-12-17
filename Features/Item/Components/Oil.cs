namespace untitledplantgame.Item.Components;

public partial class Oil : AComponent
{
	public Oil() { } //needed to instantiate the class

	public override AComponent Clone() => new Oil();
}
