namespace untitledplantgame.Common.Inputs.GameActions;

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
