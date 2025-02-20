using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;
using untitledplantgame.NPC.Routine;

namespace untitledplantgame.NPC.RoutinePlanner;

/// <summary>
///		The RoutinePlanner manages all Routines given to it.
/// </summary>
public partial class NpcRoutinePlanner : Node
{
	[Export] private Npc _npcExecutingRoutines;
	[Export] private Array<NpcRoutine> _routines;
	public INpcTask ActiveTask;
	public NpcRoutine LastRoutine; // Also saves the currently active Routine if there was no previous Routine.
	public NpcRoutine StartingRoutine;
	
	private bool _startingRoutineSet;
	private const int ScriptExecutionOrderDelay = 16;
	private Logger _logger;
	
	public override void _Ready()
	{
		ExecuteAllRoutines(); 
		_logger = new Logger(this);
	}

	/// <summary>
	///		Used to get the Npc that has this specific RoutinePlanner.
	/// </summary>
	/// <returns></returns>
	public Npc GetNpcExecutingRoutines()
	{
		return _npcExecutingRoutines;
	}
	
	/*
	 * Rider doesn't know this, but Routines are called precisely because they should be executed in a never ending loop.
	 *
	 * Npcs are programmed to start over once every routine is finished!
	 */
	private async void ExecuteAllRoutines()
	{
		await Task.Delay(ScriptExecutionOrderDelay);
		_logger.Debug("Starting to execute the Npc's routines.");

		var tasks = new List<Task>();
		
		foreach (var npcRoutine in _routines)
		{
			if (!_startingRoutineSet)
			{
				npcRoutine.InitializeRoutine(this);
				npcRoutine.MakeThisRoutineTheStartingPoint();
				StartingRoutine = npcRoutine;
				_startingRoutineSet = true;
			}
			else
			{
				npcRoutine.InitializeRoutine(this);
			}
			
			tasks.Add(npcRoutine.ExecuteAllTasks());
		}

		await Task.WhenAll(tasks);
		
		ExecuteAllRoutines();
	}
}
