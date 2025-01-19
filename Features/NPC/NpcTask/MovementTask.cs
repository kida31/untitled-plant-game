using System;
using System.Threading.Tasks;
using Godot;

namespace untitledplantgame.NPC.NpcTask;

/**
 * This task is used to move the NPC to a specified location.
 * The NPC is not using any complex algorithm to get there; it simply walks in a straight line until it reaches the destination.
 *
 * The task is finished when the NPC reaches the destination.
 */
public partial class MovementTask :  Area2D, INpcTask
{
	private Vector2 _lastValidVelocity;
	private bool DestinationReached { get; set; }
	private event EventHandler TaskStarted;
	private event EventHandler TaskFinished;
	private Npc _npcExecutingTheseTasks;
	
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public void InitializeTask(Npc owningNpc)
	{
		_npcExecutingTheseTasks = owningNpc;
	}

	public void StartTask()
	{
		TaskStarted?.Invoke(this, EventArgs.Empty);
	}

	public void FinishTask()
	{
		TaskFinished?.Invoke(this, EventArgs.Empty);
	}

	public bool IsTaskActive()
	{
		return DestinationReached;
	}

	public void InterruptCurrentTask()
	{
		_lastValidVelocity = _npcExecutingTheseTasks.Velocity;
			
		_npcExecutingTheseTasks.Velocity = Vector2.Zero;
	}

	public void ResumeCurrentTask()
	{
		_npcExecutingTheseTasks.Velocity = _lastValidVelocity;
		_lastValidVelocity = new Vector2();
	}

	private void OnBodyEntered(Node2D npc)
	{
		var npcInstance = npc as Npc;
		if (npcInstance == _npcExecutingTheseTasks)
		{
			DestinationReached = true;
			FinishTask();
		}
	}
	
	public async Task ExecuteNpcTask()
	{
		await Task.Yield();
		StartTask();
		
		var newVelocity = Position - _npcExecutingTheseTasks.Position;
		_npcExecutingTheseTasks.Velocity = newVelocity.Normalized() * 100;
		
		await WaitForConditionAsync();
		_npcExecutingTheseTasks.Velocity = Vector2.Zero;
	}
	
	private Task WaitForConditionAsync()
	{
		var tcs = new TaskCompletionSource<bool>();
		
		EventHandler onConditionMet = null;
		onConditionMet = (sender, args) =>
		{
			if (!DestinationReached)
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
