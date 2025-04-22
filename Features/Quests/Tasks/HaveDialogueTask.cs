using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class HaveDialogueTask : QuestTask
{
	[Export] private DialogueResourceObject _dialogue;
	public override bool IsCompleted => _isCompleted;
	public override event Action<QuestTask> TaskCompleted;
	private bool _isCompleted = false;
	private readonly Logger _logger = new ("QuestTask");

	public HaveDialogueTask()
	{
		EventBus.Instance.EndDialogue += DialogueEnded;
	}

	private void DialogueEnded(DialogueResourceObject obj)
	{
		if (obj != _dialogue)
		{
			return;
		}

		_isCompleted = true;
		
		EventBus.Instance.EndDialogue -= DialogueEnded;
		_logger.Debug("Dialogue ended. Quest is completed");
		TaskCompleted?.Invoke(this);
	}
}
