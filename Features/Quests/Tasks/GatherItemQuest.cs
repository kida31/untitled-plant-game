using Godot;

namespace untitledplantgame.Quests;

public partial class GatherItemQuest : QuestTask
{
	[Export] private string _itemId;
	[Export] private int _amount;
	public override void CompleteQuest()
	{
		//check if the player collected item with amount
	}
}
