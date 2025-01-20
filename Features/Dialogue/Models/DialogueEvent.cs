using System;
using Godot;

namespace untitledplantgame.Dialogue.Models;

/// <summary>
/// Base class for all Dialogue Events.
/// </summary>
[GlobalClass]
public abstract partial class DialogueEvent : DialogueResourceObject
//TODO: Remove this inheritance and replace with a common interface for event and reseource objects.
{
	/// <summary>
	/// Execute Events that are triggered by the Dialogue System on Response.
	/// </summary>
	public abstract void Execute();
}
