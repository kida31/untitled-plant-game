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

	public override void StartTask()
	{
		EventBus.Instance.EndDialogue += DialogueEnded;
	}
	
	protected override void StopTask()
	{
		EventBus.Instance.EndDialogue -= DialogueEnded;
		TaskCompleted?.Invoke(this);
	}

	private void DialogueEnded(DialogueResourceObject obj)
	{
		if (obj != Dialogue)
		{
			return;
		}

		_isCompleted = true;

		StopTask();
	}
}
