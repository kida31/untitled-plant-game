using Godot;
using Godot.Collections;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Buttons;

public partial class SwapInventoryViewUiButton : Control
{
	[Export] private string _buttonName;
	[Export] private CanvasItem _nodeToShow;
	[Export] private Array<CanvasItem> _nodesToHide;
	[Export] private Button _button;
	
	public override void _Ready()
	{
		_button.Text = _buttonName;
		_button.Pressed += OnButtonClick;
	}
	
	public void OnButtonClick()
	{
		foreach (var canvasItem in _nodesToHide)
		{
			canvasItem.Hide();
			GD.Print(canvasItem.Visible);
		}

		_nodeToShow.Show();
	}
	
	// Delete after MVP
	public override void _UnhandledInput(InputEvent @event)
	{
		if (_button.HasFocus())
		{
			if (@event is InputEventJoypadButton button)
			{
				if (button.Pressed && button.ButtonIndex == JoyButton.A)
				{
					OnButtonClick();
				}
			}
		}
	}

	public void SetFocus()
	{
		_button.GrabFocus();
	}
}
