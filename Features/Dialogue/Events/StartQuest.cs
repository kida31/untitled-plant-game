using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Quests;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class StartQuest : DialogueEvent
{
	[Export] private QuestLine QuestLine { get; set; }
	public override void Execute()
	{
		QuestController.Instance.StartQuestLine(QuestLine);
	}
}
