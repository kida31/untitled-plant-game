using Godot;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class OpenFishingGame : DialogueEvent
{
	private PackedScene _fishingGameScene = GD.Load<PackedScene>("res://Features/Fishing/FishingGame.tscn");
	public override void Execute()
	{
		GameStateMachine.Instance.ChangeState(GameState.Fishing);
		var fishingGame = _fishingGameScene.Instantiate();
		// Open the fishing game
		// This is a placeholder for the actual implementation
	}
}
