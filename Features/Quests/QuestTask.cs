using System;
using System.Xml;
using Godot;

namespace untitledplantgame.Quests;

[GlobalClass]
public abstract partial class QuestTask : Resource
{
	public abstract bool IsCompleted { get; }

	public abstract event Action<QuestTask> TaskCompleted;

	public virtual void StartTask()
	{
		// This method can be overridden by derived classes to implement task-specific logic
	}
	
	protected virtual void StopTask()
	{
		// This method can be overridden by derived classes to implement task-specific logic
		// It is called when the task is completed
	}
}
