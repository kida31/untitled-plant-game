using System;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.NPC.NpcTask;

/// <summary>
///		This task is used to move the NPC to a specified location.
///		This script is supposed to be assigned to a physical area in the game (using Area2D or other nodes).
///		The NPC is not using any complex algorithm to get there; it simply walks in a straight line until it reaches the destination.
///
///		The task is finished when the NPC reaches the destination.
/// </summary>
public partial class MovementTask :  Area2D, INpcTask
{
	private bool DestinationReached { get; set; }
	private Vector2 _lastValidVelocity;
	private Vector2 _anchorPoint; // Godot has a problem with the global position.
	private event EventHandler TaskStarted;
	private event EventHandler TaskFinished;
	private Npc _npcExecutingThisTasks;
	private Logger _logger;
	
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		_logger = new Logger(this);
	}

	public void InitializeTask(Npc owningNpc)
	{
		_npcExecutingThisTasks = owningNpc;
		_anchorPoint = _npcExecutingThisTasks.GlobalPosition;
		_logger.Debug("Task assigned " + _npcExecutingThisTasks.GetNpcName() + " as it's owner.");
	}

	public void StartTask()
	{
		TaskStarted?.Invoke(this, EventArgs.Empty);
		_logger.Info("MovementTask started.");
	}

	public void FinishTask()
	{
		TaskFinished?.Invoke(this, EventArgs.Empty);
		_logger.Info("MovementTask finished.");
	}

	public void InterruptCurrentTask()
	{
		if (_npcExecutingThisTasks.Velocity != Vector2.Zero)
		{
			_lastValidVelocity = _npcExecutingThisTasks.Velocity;
		}
		_npcExecutingThisTasks.Velocity = Vector2.Zero;
		_logger.Debug("MovementTask was interrupted!");
	}

	public void ResumeCurrentTask()
	{
		if (_lastValidVelocity != Vector2.Zero)
		{
			_npcExecutingThisTasks.Velocity = _lastValidVelocity;
		}
		
		_lastValidVelocity = new Vector2();
		_logger.Debug("MovementTask was resumed");
	}
	
	public async Task ExecuteNpcTask()
	{
		_logger.Debug("Async Task execution started.");
		await Task.Yield();
		StartTask();
		
		var newVelocity = GlobalPosition - _npcExecutingThisTasks.GlobalPosition;
		_npcExecutingThisTasks.Velocity = newVelocity.Normalized() * 100;
		
		await WaitForConditionAsync();
		_npcExecutingThisTasks.Velocity = Vector2.Zero;
	}
	
	private void OnBodyEntered(Node2D npc)
	{
		var npcInstance = npc as Npc;
		if (npcInstance == _npcExecutingThisTasks)
		{
			DestinationReached = true;
			FinishTask();
		}
	}
	
	/// <summary>
	///		Uses an EventHandler to check if a Npc has reached the specified destination.
	/// </summary>
	/// <returns></returns>
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
			_logger.Debug("The Npc reached the task's destination.");
			tcs.TrySetResult(true);
			
			TaskFinished -= onConditionMet;
		};

		TaskFinished += onConditionMet;

		return tcs.Task;
	}
}
