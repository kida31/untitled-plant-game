using System;
using Godot;

namespace untitledplantgame.Dialogue.Models;

[GlobalClass]
public partial class DialogueEvent : DialogueResourceObject
{
	public virtual void ExecuteEvent()
	{
		throw new NotImplementedException();
	}
}
