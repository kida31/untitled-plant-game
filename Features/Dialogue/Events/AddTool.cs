using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Tools;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class AddTool : DialogueEvent
{
	[Export] Tool _tool;

	public override void Execute()
	{
		var toolbelt = Game.Player.Toolbelt;
		toolbelt?.AddTool(_tool);
	}
}
