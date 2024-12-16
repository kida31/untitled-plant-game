using Godot;
using untitledplantgame.Common.Inputs.GameActions;

namespace untitledplantgame.Player;

public partial class StateWalk : State
{
	[Export]
	private float _movespeed = 100.0f;
	private State idleState;
	private State _useToolState;

	public override void _Ready()
	{
		idleState = GetNode<State>("../Idle");
		_useToolState = GetNode<State>("../UseTool");
	}

	public override void Enter()
	{
		Player.UpdateAnimation("walk");
	}

	public override State Process(double delta)
	{
		if (Player.Direction == Vector2.Zero)
			return idleState;

		Player.Velocity = Player.Direction * _movespeed;
		if (Player.SetDirection())
			Player.UpdateAnimation("walk");
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
