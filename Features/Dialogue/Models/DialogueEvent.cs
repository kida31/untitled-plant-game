using System;
using Godot;

namespace untitledplantgame.Dialogue.Models;

/// <summary>
/// Base class for all Dialogue Events.
/// </summary>
[GlobalClass]
public abstract partial class DialogueEvent : DialogueResourceObject
{
	/// <summary>
	/// Execute Events that are triggered by the Dialogue System on Response.
	/// </summary>
	public abstract void Execute();
}
