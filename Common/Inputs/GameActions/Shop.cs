namespace untitledplantgame.Common.Inputs.GameActions;

/// <summary>
///		List of shop game actions. Only active when the shop is open.
/// </summary>
public static class Shop
{
	public const string Left = "shop_left";
	public const string Right = "shop_right";
	public const string Up = "shop_up";
	public const string Down = "shop_down";
	public const string North = "shop_north";
	public const string South = "shop_south";
	public const string East = "shop_east";
	public const string West = "shop_west";
	public const string TriggerLeft = "shop_trigger_left";
	public const string TriggerRight = "shop_trigger_right";
	public const string Confirm = South;
	
	// Most inputs are handled via Godot's inbuilt GUI. These are the only ones that need to be handled manually...
	public const string CloseShop = East;
}
