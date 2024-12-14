namespace untitledplantgame.Item.Components;

public partial class Decoration : AComponent
{
	public Decoration() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
