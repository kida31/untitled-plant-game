using Godot;
using untitledplantgame.Common.Inputs.GameActions;

namespace untitledplantgame.Player;

public partial class StateUseTool : State
{
    private State _idleState;

    public override void _Ready()
    {
        _idleState = GetNode<State>("../Idle"); // string might be prone to error
    }

    public override void Enter()
    {
        Player.UpdateAnimation("idle");
    }

    public override State Process(double delta)
    {
        Player.SetDirection();
        Player.Velocity = Vector2.Zero;
        Player.Toolbelt.CurrentTool?.Use(Player); // Should be a public method in player instead of property access
        // TODO add channel time;
        return _idleState;
    }

    public override State HandleInput(InputEvent inputEvent)
    {
        Player.GetSetInputDirection();
        if (inputEvent.IsActionReleased(FreeRoam.UseTool))
        {
            return _idleState;
        }
        return null;
    }
}
