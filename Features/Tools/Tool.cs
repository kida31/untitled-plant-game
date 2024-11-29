using System.Reflection.Metadata;
using Godot;

namespace untitledplantgame.Tools;



// TODO: Implement cast time
// Problem, cast time logic needs to be in/by a Node
// Part of the logic (Use()) is in Tool. Logic should be in one place only.
public abstract class Tool
{
	private readonly float _radius;
	private readonly float _range;
	private readonly float _castTime;

    private Player.Player _user;
    private ToolHitScan _hitScan;
    
    public Tool(float radius, float range, float castTime = 0)
    {
        _radius = radius;
        _range = range;
        _castTime = castTime;
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
