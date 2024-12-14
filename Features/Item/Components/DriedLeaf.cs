namespace untitledplantgame.Item.Components;

public partial class DriedLeaf : AComponent
{
	public DriedLeaf() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
