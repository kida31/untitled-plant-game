using System.Xml;
using Godot;

namespace untitledplantgame.Quests;

public abstract partial class QuestCategory : Resource
{
	public abstract void CompleteQuest();
}
