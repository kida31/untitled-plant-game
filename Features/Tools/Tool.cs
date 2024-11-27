using Godot;

namespace untitledplantgame.Tools;

public abstract class Tool
{
    private bool _isHandled;
    protected float _radius;
    protected float _range;

    public Tool(float radius, float range)
    {
        _radius = radius;
        _range = range;
        _isHandled = false;
    }

    public void Use(Player.Player user)
    {
        _isHandled = false;
        var box = CreateHitBox(_radius);
        user.AddChild(box);
        box.GlobalPosition = user.GlobalPosition + user.Direction * _range;
    }

    protected virtual void OnUse(Player.Player user)
    {

    }

    protected virtual void OnHit(Player.Player user, Node2D entity)
    {

    }

    protected virtual void OnMiss(Player.Player user)
    {

    }

    protected void SetAsHandled()
    {
        _isHandled = true;
    }

    private Area2D CreateHitBox(float radius)
    {
        var area = new Area2D();
        var colshape = new CollisionShape2D();
        var circle = new CircleShape2D();
        circle.Radius = radius;
        colshape.Shape = circle;

        area.AddChild(colshape);
        return area;
    }
}