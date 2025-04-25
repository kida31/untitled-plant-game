using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class HaveDialogueTask : QuestTask
{
	[Export] public DialogueResourceObject Dialogue { get; private set; }
	public override bool IsCompleted => _isCompleted;
	public override event Action<QuestTask> TaskCompleted;
	private bool _isCompleted = false;

	public HaveDialogueTask()
	{
		EventBus.Instance.EndDialogue += DialogueEnded;
	}

	private void DialogueEnded(DialogueResourceObject obj)
	{
		if (obj != Dialogue)
		{
			return;
		}

		_isCompleted = true;
		
		EventBus.Instance.EndDialogue -= DialogueEnded;
		TaskCompleted?.Invoke(this);
	}
}
