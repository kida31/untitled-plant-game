using Godot;
using System;
using untitledplantgame.Common;

public partial class Bed : AInteractable
{
	public string ActionName => "Sleep";
	
	public override void Interact()
	{
		TimeController.Instance.FastForwardToNextDay();
	}
}
