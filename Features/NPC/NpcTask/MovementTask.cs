using System;
using System.Threading.Tasks;
using Godot;

namespace untitledplantgame.NPC.NpcTask;

public partial class MovementTask :  Area2D, INpcTask
{
	public event Action TaskStarted;
	public event Action TaskFinished;
	
	public void StartTask()
	{
		TaskStarted?.Invoke();
	}

	public void FinishTask()
	{
		TaskFinished?.Invoke();
	}

	public async Task ExecuteNpcTask()
	{
		await Task.Delay(1000);
		GD.Print("ZOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM");
	}
}
