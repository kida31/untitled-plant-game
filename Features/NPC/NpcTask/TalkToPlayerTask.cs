using System;
using System.Threading.Tasks;
using Godot;

namespace untitledplantgame.NPC.NpcTask;

public partial class TalkToPlayerTask :  Node, INpcTask
{
	[Export] private string _text;

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
		GD.Print(_text);
	}
}
