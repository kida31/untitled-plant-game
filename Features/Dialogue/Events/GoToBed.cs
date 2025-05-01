using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue.Events;

/// <summary>
///		 Event that transitions to the start of the next day.
/// </summary>
[GlobalClass]
public partial class GoToBed : DialogueEvent
{
	public override void Execute()
	{
		_ = DoAsyncStuff();
	}

	private async Task DoAsyncStuff()
	{
		// TODO: Cleanup placeholders
		GameStateMachine.Instance.ChangeState(GameState.Config); // Placeholder for "do not move"
		await SceneTransition.Instance.FadeIn();
		await TimeController.Instance.GoToNextDay();
		await SceneTransition.Instance.FadeOut();
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam); // Placeholder for "do not move"
	}
}
