using System;
using System.Collections.Generic;
using Godot;

// Tool HITSCAN
public partial class ToolHitScan : Area2D
{
    private const float ScanTimeSeconds = 100 / 1000f;
    public event Action<Node2D[]> Hit;

    public ToolHitScan(float radius) : base()
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
        timer.Timeout += OnTimeout;
        AddChild(timer);
    }

	private void OnTimeout()
	{
		var nodes = new List<Node2D>();
        nodes.AddRange(GetOverlappingAreas());
        nodes.AddRange(GetOverlappingBodies());
        Hit?.Invoke(nodes.ToArray());
        QueueFree();
	}

	public ToolHitScan() : this(10) { }


}