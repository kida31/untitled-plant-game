using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC;

namespace untitledplantgame.Player;

public partial class CollisionManager : Node
{
	public static CollisionManager Instance { get; private set; }
	
	private Dictionary<string, Action<Npc>> _npcDialogueActions;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		if (Instance != null)
		{
			_logger.Warn("Multiple instances of TimeController found, deleting the new one");
			QueueFree();
			return;
		}

		Instance = this;
		
		_npcDialogueActions = new Dictionary<string, Action<Npc>>
		{
			{ "Pan Dan", ShowSpeechBubble},
			{ "Vending Machine", ShowSpeechBubble}
		};
	}

	// replace the string with npc
	public void HandleNpcCollision(Npc npc)
	{
		if (_npcDialogueActions.TryGetValue(npc.GetNpcName(), out var action))
		{
			action.Invoke(npc);
		}
		else
		{
			_logger.Debug("Collision with a none Npc detected.");
		}
	}

	private async void ShowSpeechBubble(Npc npc)
	{
		var totalMinutes = (int)(TimeController.Instance.CurrentSeconds / 60);
		var currentDayMinutes = totalMinutes % (24 * 60);

		if (currentDayMinutes is <= 1380 and >= 300)
		{
			return;
		}

		foreach (var node in npc.GetChildren())
		{
			if (node.Name != "SpeechBubble" || node is not Node2D speechBubble)
			{
				continue;
			}

			speechBubble.Visible = true;
			await Task.Delay(2000);
			speechBubble.Visible = false;
		}
	}
}
