using Godot;

namespace untitledplantgame.Tutorial_Player;

public partial class StateIdle : State
{
    private State walkState;
    
    public override void _Ready()
    {
        walkState = GetNode<State>("../Walk");
        GD.Print(walkState);
    }

    public override void Enter()
    {
        Player.UpdateAnimation("idle");
    }

    public override State Process(double delta)
    {
        if (Player._direction != Vector2.Zero) return walkState;
        
        Player.Velocity = Vector2.Zero;
        return null;
    }
}