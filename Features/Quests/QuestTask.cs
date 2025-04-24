using System;
using System.Xml;
using Godot;

namespace untitledplantgame.Quests;

[GlobalClass]
public abstract partial class QuestTask : Resource
{
	public abstract bool IsCompleted { get; }
	
	public abstract event Action<QuestTask> TaskCompleted;
}
