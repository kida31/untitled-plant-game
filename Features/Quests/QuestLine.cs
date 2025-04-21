using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class QuestLine : Resource
{
	[Export] public string Name { get; set; }
	[Export] public string Description { get; set; }
	[Export] public Array<Quest> Quests { get; set; }

	public QuestLine()
	{
	}

	private bool IsCompleted()
	{
		return Quests.All(quest => quest.Progression == QuestProgression.Completed);
	}
}
