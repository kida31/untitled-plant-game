using System;
using Godot;
using untitledplantgame.Common;

public partial class ShopSlot : Panel
{
	[Export]
	private Label ItemName;
	private readonly Logger _logger = new("ShopSLot");

	private PackedScene tooltip = GD.Load<PackedScene>("res://Seedshop/Tooltip.tscn");

	public override void _Ready()
	{
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
	}

	private async void OnMouseEntered()
	{
		var tooltipInstance = tooltip.Instantiate<Tooltip>();
		// _logger.Debug("Mouse entered");
		tooltipInstance.origin = "Seedshop";
		tooltipInstance.slot = ItemName.Text;

		float x = GlobalPosition.X + GetRect().Size.X;
		float y = GlobalPosition.Y;
		Vector2I position = (Vector2I)new Vector2(x, y);
		tooltipInstance.Position = position;
		tooltipInstance.Transparent = true;

		AddChild(tooltipInstance);
		await ToSignal(GetTree().CreateTimer(5.0f), "timeout");
		if (HasNode("Tooltip") && tooltipInstance.valid)
		{
			tooltipInstance.Show();
		}
	}

	private void OnMouseExited()
	{
		if (HasNode("Tooltip"))
		{
			GetNode("Tooltip").QueueFree();
			_logger.Debug("Tooltip removed");
		}
		else
		{
			_logger.Debug("Tooltip node not found");
		}
	}
}
