using System;
using Godot;

// Tool HITSCAN
public partial class ToolHitBox : Area2D
{
    private const float ScanTimeSeconds = 100 / 1000;
    public event Func<Node2D, bool> Hit;
    public int HitCount { get; private set; } = 0;

    public ToolHitBox(float radius) : base()
    {
        var colshape = new CollisionShape2D();
        var circle = new CircleShape2D
        {
            Radius = radius
        };
        colshape.Shape = circle;
        AddChild(colshape);

        var timer = new Timer();
        timer.WaitTime = ScanTimeSeconds;
        timer.Autostart = true;
        AddChild(timer);
        timer.Timeout += OnTimeout;
    }

	private void OnTimeout()
	{
		throw new NotImplementedException();
	}

	public ToolHitBox() : this(10) { }

    public override void _Ready()
    {
        AreaEntered += InvokeHit;
        BodyEntered += InvokeHit;
    }

    public override void _ExitTree()
    {
        AreaEntered -= InvokeHit;
        BodyEntered -= InvokeHit;
    }

    private void InvokeHit(Node2D something)
    {        
        var wasHit = Hit?.Invoke(something);
        if (wasHit is bool b_wasHit && b_wasHit) {
            HitCount += 1;
        }
    }
}