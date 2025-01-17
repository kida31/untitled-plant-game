using System;
using System.Threading.Tasks;
using Godot;

namespace untitledplantgame.NPC.NpcTask;

public partial class MovementTask :  Area2D, INpcTask
{
	private event EventHandler TaskStarted;
	private event EventHandler TaskFinished;
	private bool _destinationReached;
	private Npc _npcExecutingTheseTasks;
	
	public override void _Ready()
	{
		_npcExecutingTheseTasks = FindNpcByName(GetParent(), "Jeff the Landshark");
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}
	
	public void StartTask()
	{
		TaskStarted?.Invoke(this, EventArgs.Empty);
	}

	public void FinishTask()
	{
		TaskFinished?.Invoke(this, EventArgs.Empty);
	}

	private void OnBodyEntered(Node2D npc)
	{
		var npcInstance = npc as Npc;
		if (npcInstance == _npcExecutingTheseTasks)
		{
			_destinationReached = true;
			FinishTask();
		}
	}

	private void OnBodyExited(Node2D npc)
	{
		
	}
	
	private Npc FindNpcByName(Node currentNode, string targetName)
	{
		var parent = (Npc) currentNode.GetParent();

		while (parent != null)
		{
			if (parent.GetNpcName() == targetName)
			{
				return parent;
			}
			parent = (Npc) parent.GetParent();
		}

		return null;
	}

	public async Task ExecuteNpcTask()
	{
		await Task.Yield();
		StartTask();
		
		var npc = (Npc) GetParent().GetParent();
		var newVelocity = Position - npc.Position;
		npc.Velocity = newVelocity.Normalized() * 100;
		
		await WaitForConditionAsync();
		npc.Velocity = Vector2.Zero;
	}
	
	private Task WaitForConditionAsync()
	{
		var tcs = new TaskCompletionSource<bool>();
		
		EventHandler onConditionMet = null;
		onConditionMet = (sender, args) =>
		{
			if (!_destinationReached)
			{
				return;
			}

			tcs.TrySetResult(true);
			TaskFinished -= onConditionMet;
		};

		TaskFinished += onConditionMet;

		return tcs.Task;
	}
}
