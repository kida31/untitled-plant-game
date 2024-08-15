using Godot;

namespace untitledplantgame.Tutorial_Player;

public partial class StateIdle : State
{
    private State _walkState;
    
    public override void _Ready()
    {
        _walkState = GetNode<State>("../Walk");
        GD.Print(_walkState);
    }

    public override void Enter()
    {
        Player.UpdateAnimation("idle");
    }

    public override State Process(double delta)
    {
        if (Player.Direction != Vector2.Zero) return _walkState;
        
        Player.Velocity = Vector2.Zero;
        return null;
    }
}