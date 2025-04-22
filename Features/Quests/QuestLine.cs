using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class QuestLine : Resource
{
	[Export] public string Name { get; set; }
	//[Export] public string Description { get; set; }
	[Export] public Array<Quest> Quests { get; set; }
	
	private Logger _logger = new ("QuestLine");

	public QuestLine()
	{
		
	}
	
	private bool IsCompleted()
	{
		_logger.Debug("Quest " + Name + " is completed");
		return Quests.All(quest => quest.Progression == QuestProgression.Completed);
	}
}
