using Godot;
using untitledplantgame.Common.Inputs.GameActions;

namespace untitledplantgame.Player;

public partial class StateIdle : State
{
	private State _walkState;
	private State _useToolState;

	public override void _Ready()
	{
		_walkState = GetNode<State>("../Walk");
		_useToolState = GetNode<State>("../UseTool");
	}

	public override void Enter()
	{
		Player.UpdateAnimation("idle");
	}

	public override State Process(double delta)
	{
		if (Player.Direction != Vector2.Zero)
			return _walkState;

		Player.Velocity = Vector2.Zero;
		return null;
	}

	public override State HandleInput(InputEvent inputEvent)
	{
		Player.GetSetInputDirection();
		if (inputEvent.IsActionPressed(FreeRoam.SwitchToNextTool))
		{
			Player.Toolbelt.GoToNext();
		}

		if (inputEvent.IsActionPressed(FreeRoam.SwitchToPreviousTool))
		{
			Player.Toolbelt.GoToPrevious();
		}

		if (inputEvent.IsActionPressed(FreeRoam.UseTool))
		{
			return _useToolState;
		}
		// Could be if-else's, or maybe not?
		return null;
	}
}
