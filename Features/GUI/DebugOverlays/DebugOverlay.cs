using System.Linq;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.DebugOverlays;

/// <summary>
///		Parent node for debug overlay objects. Contains logic for toggling self and child visibility.
/// </summary>
public partial class DebugOverlay : Control
{
	public const string DebugSettingKey = "untitled_plant_game/show_debug_overlays";

	[Export] private Key _toggleKey = Key.F3;
	[Export] private Key _toggleModifier = Key.Shift;

	private Logger _logger;

	private readonly Control _container;

	public DebugOverlay()
	{
		_container = this; // Replace this if node tree structure changes
	}

	public override void _Ready()
	{
		_logger = new Logger(this);
		Visible = ProjectSettings.GetSetting(DebugSettingKey, false).AsBool(); // default is false
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventKey {Pressed: true} keyEvent)
		{
			return;
		}

		if (keyEvent.Keycode == _toggleKey)
		{
			ToggleAll();
			Show();
			return;
		}

		if (!Input.IsKeyPressed(_toggleModifier) || keyEvent.Keycode is < Key.Key1 or > Key.Key9)
		{
			return;
		}

		// 1 to 9
		var index = (int) (keyEvent.Keycode - Key.Key1);
		ToggleChildVisibility(index);
		Show();
	}

	/// <summary>
	///		Toggle Visibility for all children depending on whatever makes sense for majority (arbitrary choice)
	/// </summary>
	private void ToggleAll()
	{
		var children = _container.GetChildren().OfType<Control>().Select(c => c.Visible).ToList();
		var majority = children.Count(v => v) > children.Count(v => !v);
		var newVisibility = !majority || !Visible; // If self is hidden, show all. If self is shown, use majority

		_logger.Debug(newVisibility ? "Show all" : "Hide all");
		foreach (var control in _container.GetChildren().OfType<Control>())
		{
			control.Visible = newVisibility;
		}
	}

	private void ToggleChildVisibility(int idx)
	{
		if (_container.GetChildCount() <= idx) return;

		var child = _container.GetChild<Control>(idx);
		_logger.Debug(!child.Visible ? $"Hide i={idx}" : $"Show i={idx}");
		child.Visible = !child.Visible;
	}
}
