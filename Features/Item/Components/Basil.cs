namespace untitledplantgame.Item.Components;

public partial class Basil : AComponent
{
	public Basil() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
