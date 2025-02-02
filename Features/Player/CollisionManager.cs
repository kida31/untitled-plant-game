using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC;
using untitledplantgame.ProximityCollision;

namespace untitledplantgame.Player;

public partial class CollisionManager : Node
{
	public static CollisionManager Instance { get; private set; }
	
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
	}

	// replace the string with npc
	public void HandleNpcCollision(Npc npc)
	{
		// ...
		
		foreach (var node in npc.GetChildren())
		{
			if (node is SpeechBubble speechBubble)
			{
				speechBubble.OnProximityEntered();
			}
		}
	}
}
