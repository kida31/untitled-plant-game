using Godot;

namespace untitledplantgame.Tutorial_Player;

public partial class StateIdle : State
{
    public override void Enter()
    {
        Player.UpdateAnimation("idle");
    }

    public override State Process(double delta)
    {
        Player.Velocity = Vector2.Zero;
        return null;
    }
}