using Godot;

namespace untitledplantgame.TestScenes;

public partial class LayerOne : Node
{
	public override void _Ready()
	{
		FocusManager.Instance.RegisterLayer(1, this);
	}

	private void OnInput(InputEvent @event)
	{
		// GetNode<Node>("../LayerZeroExample")._Input(@event);
		if (@event is InputEventMouseButton gamepadMotion)
		{
			//GD.Print(gamepadMotion);
		}
	}
}
