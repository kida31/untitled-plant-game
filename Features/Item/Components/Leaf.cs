namespace untitledplantgame.Item.Components;

public partial class Leaf : AComponent
{
	public Leaf() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
