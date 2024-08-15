using Godot;
using System;

namespace untitledplantgame.Tutorial_Player;

public partial class StateWalk : State
{
    [Export] private float _movespeed = 100.0f;
    private State idleState;


    public override void _Ready()
    {
        idleState = GetNode<State>("../Idle");
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
        if(Player.SetDirection())
            Player.UpdateAnimation("walk");
        return null;
    }
}