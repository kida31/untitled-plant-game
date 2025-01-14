using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.NPC.Routine;

namespace untitledplantgame.NPC.RoutinePlanner;

public partial class NpcRoutinePlanner : Node
{
	[Export] private Array<NpcRoutine> _routines;

	public override void _Ready()
	{
		_ = ExecuteAllRoutines();
	}

	private async Task ExecuteAllRoutines()
	{
		foreach (var npcRoutine in _routines)
		{
			await npcRoutine.ExecuteAllTasks();
		}
	}
}
