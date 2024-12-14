namespace untitledplantgame.Item.Components;

public partial class Antioxidant : AComponent
{
	public Antioxidant() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
