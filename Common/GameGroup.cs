namespace untitledplantgame.Common;

/// <summary>
/// Common names used as Godot Groups throughout the game.
/// <para>
/// <remarks>
/// Groups should be added to nodes via code for tracability. Avoid adding them to a scene via Editor.
/// </remarks>
/// </para>
/// <para>
/// Utilize scene tree to
/// <list type="bullet">
/// <item><description>Get a list of nodes in a group.</description></item>
/// <item><description>ICall a method on all nodes in a group.</description></item>
/// <item><description>Send a notification to all nodes in a group.</description></item>
/// </list>
/// <see href="https://docs.godotengine.org/en/stable/tutorials/scripting/groups.html">Godot Docs about Groups</see>
/// </para>
/// </summary>
public class GameGroup
{
	/// <summary>
	/// Refers to the player node. See <see cref="untitledplantgame.Player.Player">Player Class</see>
	/// </summary>
	public const string Player = "player";
	public const string Interactables = "interactables";
	public const string Plants = "plants";
	public const string Soil = "soil";
}
