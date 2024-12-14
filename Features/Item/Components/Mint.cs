namespace untitledplantgame.Item.Components;

public partial class Mint : AComponent
{
	public Mint() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
