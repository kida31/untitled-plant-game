using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class GoToBed : DialogueEvent
{
	private const int TransitionDuration = 1500;
	
	public override void Execute()
	{
		DoAsyncStuff();
	}

	private async Task DoAsyncStuff()
	{
		// TODO: Cleanup placeholders
		GameStateMachine.Instance.ChangeState(GameState.Config); // Placeholder for "do not move"
		await SceneTransition.Instance.FadeIn();
		TimeController.Instance.GoToNextDay();
		await Task.Delay(TransitionDuration);
		await SceneTransition.Instance.FadeOut();
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam); // Placeholder for "do not move"
	}
}
