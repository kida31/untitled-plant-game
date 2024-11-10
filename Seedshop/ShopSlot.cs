using System;
using Godot;
using untitledplantgame.Common;

public partial class ShopSlot : Panel
{
	private readonly Logger _logger = new("Seedshop");

	PackedScene tooltip = GD.Load<PackedScene>("res://Seedshop/Tooltip.tscn");

	public override void _Ready()
	{
		Connect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
		Connect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
	}

	public override void _Process(double delta) { }

	private async void OnMouseEntered()
	{
		_logger.Debug("Mouse entered");
		var tooltipInstance = (Tooltip)tooltip.Instantiate();
		tooltipInstance.origin = "Seedshop";
		tooltipInstance.slot = GetNode<Label>("Panel/Name").Text;
		_logger.Debug("Slot name: " + tooltipInstance.slot);
		_logger.Debug("Tooltip origin: " + tooltipInstance.origin);
		// tooltipInstance.Position = (Vector2I)GetGlobalMousePosition();

		AddChild(tooltipInstance);
		_logger.Debug("Tooltip valid: " + tooltipInstance.valid);

		_logger.Debug("has node: " + HasNode("Tooltip"));
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		if (HasNode("Tooltip") && tooltipInstance.valid)
		{
			tooltipInstance.Show();
		}
	}

	private void OnMouseExited()
	{
		_logger.Debug("Mouse exited");
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
