using System.Collections.Generic;
using Godot;

namespace untitledplantgame.Quests;

public partial class QuestController : Node
{
	private QuestLine _currentQuestLine;
	private Dictionary<string, Quest> _activeQuests = new Dictionary<string, Quest>();
}
