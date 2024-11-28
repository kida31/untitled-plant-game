using System.Reflection.Metadata;
using Godot;

namespace untitledplantgame.Tools;

public abstract class Tool
{
    protected float _radius;
    protected float _range;

    private Player.Player _user;
    private ToolHitScan _hitScan;


    public Tool(float radius, float range)
    {
        _radius = radius;
        _range = range;
    }

    public void Use(Player.Player user)
    {
        _hitScan = null;
        _user = user;
        
        OnUse(user);

        var box = new ToolHitScan(_radius);
        box.TopLevel = true;
        box.Hit += (hits) => _OnHit(user, hits);
        user.AddChild(box);
        box.GlobalPosition = user.GlobalPosition + user.FrontDirection * _range;
    }

    protected abstract void OnUse(Player.Player user);
    protected abstract bool OnHit(Player.Player user, Node2D[] hits);
    protected abstract void OnMiss(Player.Player user);

    private void _OnHit(Player.Player user, Node2D[] hits) {
        if (hits.Length == 0 || !OnHit(user, hits)) {
            OnMiss(user);
        }
    }
}