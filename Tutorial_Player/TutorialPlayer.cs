using System;
using Godot;

namespace untitledplantgame.Tutorial_Player;

public partial class TutorialPlayer : CharacterBody2D
{
    Vector2 _cardinalDirection = Vector2.Down;
    public Vector2 _direction = Vector2.Zero;
    
    private AnimatedSprite2D _animatedSprite2D;
    private PlayerStateMachine _stateMachine;
    
    public override void _Ready()
    {
        _stateMachine = GetNode<PlayerStateMachine>("StateMachine");
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _stateMachine.Initialize(this);
    }

    public override void _Process(double delta)
    {
        _direction.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        _direction.Y = Input.GetActionStrength("down") - Input.GetActionStrength("up");

        //Velocity = direction * MoveSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public bool SetDirection()
    {
        Vector2 newDirection = _cardinalDirection;

        if (_direction == Vector2.Zero) return false;

        if (_direction.Y == 0)
        {
            if (_direction.X < 0) newDirection = Vector2.Left;
            else newDirection = Vector2.Right;
        }

        if (_direction.X == 0)
        {
            if (_direction.Y > 0) newDirection = Vector2.Down;
            else newDirection = Vector2.Up;
        }

        _cardinalDirection = newDirection;
        return true;
    }

    public void UpdateAnimation(string state)
    {
        var animationState = state + "_" + AnimationDirection();
        _animatedSprite2D.Play(animationState);
    }

    string AnimationDirection()
    {
        if (_cardinalDirection == Vector2.Down)
            return "down";
        if (_cardinalDirection == Vector2.Up)
            return "up";
        if (_cardinalDirection == Vector2.Left)
            return "left";
        if (_cardinalDirection == Vector2.Right)
            return "right";
        
        else return "err";
        
    }}