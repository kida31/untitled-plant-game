namespace untitledplantgame.Item.Components;

public partial class Sunflower : AComponent
{
	public Sunflower() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
