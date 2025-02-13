using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Player;

public partial class IntroMonologueTask : Node
{
	[Export] private DialogueResourceObject _monologueResourceObject;
	[Export] private bool _skipDialogue;
	
	private const int DelayAmount = 100;
	
	public override void _Ready()
	{
		if (_skipDialogue)
		{
			return;
		}
		//WaitForSmoothness();
		EventBus.Instance.InvokeStartingDialogue(_monologueResourceObject);
	}

	private async void WaitForSmoothness()
	{
		await Task.Delay(DelayAmount);
		EventBus.Instance.InvokeStartingDialogue(_monologueResourceObject);
	}
}
