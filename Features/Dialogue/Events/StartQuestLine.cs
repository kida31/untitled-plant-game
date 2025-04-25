using Godot;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Quests;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class StartQuestLine : DialogueEvent
{
	[Export] private QuestLine QuestLine { get; set; }
	public override void Execute()
	{
		QuestController.Instance.StartQuestLine(QuestLine);
	}
}
