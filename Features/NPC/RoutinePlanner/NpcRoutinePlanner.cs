using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;
using untitledplantgame.NPC.Routine;

namespace untitledplantgame.NPC.RoutinePlanner;

public partial class NpcRoutinePlanner : Node
{
	[Export] private Npc _npcExecutingRoutines;
	[Export] private Array<NpcRoutine> _routines;
	public INpcTask ActiveTask;

	private bool _startingRoutineSet;
	private const int ScriptExecutionOrderDelay = 16;
	private Logger _logger;
	
	public override void _Ready()
	{
		ExecuteAllRoutines(); 
		_logger = new Logger(this);
	}

	public Npc GetNpcExecutingRoutines()
	{
		return _npcExecutingRoutines;
	}
	
	/*
	 * Rider doesn't know this, but routines are called routines precisely because they are done in an endless loop.
	 *
	 * Npc are programmed to start over once every routine is finished!
	 */
	private async void ExecuteAllRoutines()
	{
		await Task.Delay(ScriptExecutionOrderDelay);
		_logger.Debug("Starting to execute the Npc's routines.");
		
		//EventBus.Instance.NpcDialogueWithPlayerStarted(
		//	(AnimatedSprite2D) _npcExecutingRoutines.GetNode("PortraitSprite2D"),
		//	_npcExecutingRoutines.GetNpcName()
		//);

		var tasks = new List<Task>();
		
		foreach (var npcRoutine in _routines)
		{
			if (!_startingRoutineSet)
			{
				npcRoutine.InitializeRoutine(this);
				npcRoutine.MakeThisRoutineTheStartingPoint();

				_startingRoutineSet = true;
			}
			else
			{
				npcRoutine.InitializeRoutine(this);
			}
			
			//await npcRoutine.ExecuteAllTasks();
			tasks.Add(npcRoutine.ExecuteAllTasks());
		}

		await Task.WhenAll(tasks);
		
		ExecuteAllRoutines();
	}
}
