namespace untitledplantgame.Item.Components;

public partial class Rose : AComponent
{
	public Rose() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
