using Godot;

namespace untitledplantgame.Common.Inputs;

/// <summary>
/// Const string collection of all actions in the game
/// </summary>
public abstract class UPGActions
{
	/// <summary>
	/// Returns they Key that is mapped to this action
	/// </summary>
	/// <param name="actionName"></param>
	/// <returns></returns>
	public static Key GetKey(string actionName) => ControlScheme.GetKey(actionName);
	/// <summary>
	/// Returns they gamepad button that is mapped to this action
	/// </summary>
	/// <param name="actionName"></param>
	/// <returns></returns>
	public static JoyButton GetButton(string actionName) => ControlScheme.GetButton(actionName);
	
	public const string UIAccept = "ui_accept";
	public const string UISelect = "ui_select";
	
	public abstract class FreeRoam
	{
		public const string Left = "freeroam_left";
		public const string Right = "freeroam_right";
		public const string Up = "freeroam_up";
		public const string Down = "freeroam_down";

		public const string North = "freeroam_north";
		public const string South = "freeroam_south";
		public const string East = "freeroam_east";
		public const string West = "freeroam_west";

		public const string TriggerLeft = "freeroam_trigger_left";
		public const string TriggerRight = "freeroam_trigger_right";

		public const string Interact = South;
		public const string UseTool = West;
		public const string OpenBook = North;

		public const string SwitchToPreviousTool = TriggerLeft;
		public const string SwitchToNextTool = TriggerRight;
	}

	public abstract class BookInventory
	{
		public const string Left = "book_left";
		public const string Right = "book_right";
		public const string Up = "book_up";
		public const string Down = "book_down";

		public const string North = "book_north";
		public const string South = "book_south";
		public const string East = "book_east";
		public const string West = "book_west";

		public const string TriggerLeft = "book_trigger_left";
		public const string TriggerRight = "book_trigger_right";

		public const string Confirm = South;
		public const string CloseBook = North;
		public const string Back = East;
		public const string Special = West;
	}

	public abstract class Shop
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
		public const string CloseShop = East;
	}

	public abstract class Chest
	{
		public const string Left = "chest_left";
		public const string Right = "chest_right";
		public const string Up = "chest_up";
		public const string Down = "chest_down";

		public const string North = "chest_north";
		public const string South = "chest_south";
		public const string East = "chest_east";
		public const string West = "chest_west";

		public const string TriggerLeft = "chest_trigger_left";
		public const string TriggerRight = "chest_trigger_right";

		public const string Confirm = South;
		public const string CloseChest = East;
		public const string PickUp = West;
	}

	public abstract class Dialogue
	{
		public const string Left = "dialogue_left";
		public const string Right = "dialogue_right";
		public const string Up = "dialogue_up";
		public const string Down = "dialogue_down";

		public const string North = "dialogue_north";
		public const string South = "dialogue_south";
		public const string East = "dialogue_east";
		public const string West = "dialogue_west";

		public const string TriggerLeft = "dialogue_trigger_left";
		public const string TriggerRight = "dialogue_trigger_right";

		public const string Confirm = South;
	}
}
