using Godot;
using System;

public partial class TutorialPlayer : CharacterBody2D
{
    private float MoveSpeed = 100.0f;
    Vector2 CardinalDirection = Vector2.Down;
    private Vector2 direction = Vector2.Zero;
    private String State = "idle";
    
    private AnimatedSprite2D _animatedSprite2D;
    
    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        direction.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        direction.Y = Input.GetActionStrength("down") - Input.GetActionStrength("up");

        Velocity = direction * MoveSpeed;

        if(SetState() || SetDirection()) UpdateAnimation();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    bool SetDirection()
    {
        Vector2 new_direction = CardinalDirection;

        if (direction == Vector2.Zero) return false;

        if (direction.Y == 0)
        {
            if (direction.X < 0) new_direction = Vector2.Left;
            else new_direction = Vector2.Right;
        }

        if (direction.X == 0)
        {
            if (direction.Y > 0) new_direction = Vector2.Down;
            else new_direction = Vector2.Up;
        }

        CardinalDirection = new_direction;
        return true;
    }

    bool SetState()
    {
        String new_state;
        if (direction == Vector2.Zero)
        {
            new_state = "idle";
        }
        else new_state = "walk";
        if(State == new_state) return false;

        State = new_state;
        return true;
    }

    void UpdateAnimation()
    {
        String animationState = State + "_" + AnimationDirection();
        _animatedSprite2D.Play(animationState);
        GD.Print(animationState);
    }

    String AnimationDirection()
    {
        if (CardinalDirection == Vector2.Down)
            return "down";
        if (CardinalDirection == Vector2.Up)
            return "up";
        if (CardinalDirection == Vector2.Left)
            return "left";
        if (CardinalDirection == Vector2.Right)
            return "right";
        
        else return "err";
        
    }}
