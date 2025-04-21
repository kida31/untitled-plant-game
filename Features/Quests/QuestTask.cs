using System.Xml;
using Godot;

namespace untitledplantgame.Quests;

[GlobalClass]
public abstract partial class QuestTask : Resource
{
	public abstract void CompleteQuest();
}
