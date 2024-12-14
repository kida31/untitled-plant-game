namespace untitledplantgame.Item.Components;

public partial class Oil : AComponent
{
	public Oil() {} //needed to instantiate the class
	
	public override AComponent CombineComponent(AComponent otherComponent)
	{
		return otherComponent.GetType() == GetType() ? this : null;
	}
}
