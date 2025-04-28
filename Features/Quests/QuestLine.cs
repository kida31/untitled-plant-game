using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class QuestLine : Resource
{
	[Export] public string Id { get; private set; }
	[Export] public string Name { get; private set; }
	[Export] public Array<Quest> Quests { get; private set; }

	private readonly Logger _logger = new("QuestLine");

	public QuestLine()
	{
	}

	private bool IsCompleted()
	{
		_logger.Debug("Quest " + Name + " is completed");
		return Quests.All(quest => quest.Progression == QuestProgression.Completed);
	}
}
