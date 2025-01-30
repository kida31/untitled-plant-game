using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcType;

public partial class NoteNpc : Npc
{
	[Export] private string _name;
	[Export] private bool _disableMovement;
	[Export] private NpcPlayerInteraction _npcPlayerInteraction;
	[Export] private DialogueResourceObject _normalDialogue;
	[Export] private DialogueResourceObject _nightDialogue;

	private Npc _npcExecutingTheseTasks;
	private Logger _logger;
	
	public override void _Ready()
	{
		base._Ready();
		_logger = new Logger(this);
		_npcPlayerInteraction.InteractionEvent += SecretNoteToPlayer;
	}
	
	
	public override string GetNpcName()
	{
		_logger.Info("Npc name was requested.");
		return _name;
	}

	private void SecretNoteToPlayer()
	{
		var totalMinutes = (int)(TimeController.Instance.CurrentSeconds / 60);
		var currentDayMinutes = totalMinutes % (24 * 60);

		EventBus.Instance.InvokeStartingDialogue(currentDayMinutes is <= 1380 and >= 300 ? _normalDialogue : _nightDialogue);
	}
}
