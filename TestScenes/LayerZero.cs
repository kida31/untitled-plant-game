using Godot;
	
namespace untitledplantgame.TestScenes;

public partial class LayerZero : Node
{
	public override void _Ready()
	{
		FocusManager.Instance.RegisterLayer(0, this);
	}

	private void OnInput(InputEvent @event)
	{
		if (@event is InputEventKey mouseButton)
		{
			//GD.Print(mouseButton);
		}
	}
}
