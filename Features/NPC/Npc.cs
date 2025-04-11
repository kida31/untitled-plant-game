using Godot;

namespace untitledplantgame.NPC;

/// <summary>
///		A very basic class which is used as the basis for all Npcs.
/// </summary>
public abstract partial class Npc : CharacterBody2D
{
	/// <summary>
	///		Used to get the Name of the Npc.
	/// </summary>
	/// <returns></returns>
	public abstract string GetNpcName();
}
