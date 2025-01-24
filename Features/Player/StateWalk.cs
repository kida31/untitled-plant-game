using Godot;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Interaction;

namespace untitledplantgame.Player;

public partial class StateWalk : State
{
	[Export]
	private float _movespeed = 100.0f;
	private State _idleState;
	private State _useToolState;

	public override void _Ready()
	{
		_idleState = GetNode<State>("../Idle");
		_useToolState = GetNode<State>("../UseTool");
	}

	public override void Enter()
	{
		Player.UpdateAnimation("walk");
	}

	public override State Process(double delta)
	{
		if (Player.Direction == Vector2.Zero)
		{
			return _idleState;
		}
			

		Player.Velocity = Player.Direction * _movespeed;
		Player.UpdateAnimation("walk");
		return null;
	}

	public override State HandleInput(InputEvent inputEvent)
	{
		Player.GetSetDirection();
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

		if (inputEvent.IsActionPressed(FreeRoam.Interact))
		{
			InteractionManager.Instance.PerformInteraction();
		}
		// Could be if-else's, or maybe not?
		return null;
	}
}
