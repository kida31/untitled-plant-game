namespace untitledplantgame.Item.Components;

public partial class Lavender : AComponent
{
	public Lavender() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
