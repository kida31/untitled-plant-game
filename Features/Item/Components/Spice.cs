namespace untitledplantgame.Item.Components;

public partial class Spice : AComponent
{
	public Spice() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
