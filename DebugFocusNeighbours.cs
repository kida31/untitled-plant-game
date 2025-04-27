using Godot;
using System;
using untitledplantgame.Common;

public partial class DebugFocusNeighbours : Control
{
    private Viewport _vp;
    private Logger _logger;

    public override void _Ready()
    {
        _logger = new(this);
        VisibilityChanged += PrintFocus;
        var vp = GetViewport();
        vp.GuiFocusChanged += (_) => PrintFocus();
    }

	public override void _Input(InputEvent @event)
	{
        // TODO Remove
		if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Backspace) {
            PrintFocus();
        }
	}

	private void PrintFocus() {
        _vp ??= GetViewport();

        var ctrl = _vp.GuiGetFocusOwner();

        if (ctrl == null)
        {
            _logger.Info("Nothing focused");
        }
        else
        {
            _logger.Info($"Focused: {ctrl}");
            string NeighbourAsString(NodePath np, Side side) {
                if (np != null && !np.IsEmpty) {
                    return $"{ctrl.GetNode(np)}";
                } else {
                    return $"{ctrl.FindValidFocusNeighbor(side)} (auto)";
                }
            }

            _logger.Info(
                $"Left:{NeighbourAsString(ctrl.FocusNeighborLeft, Side.Left)}\n" +
                $"Right:{NeighbourAsString(ctrl.FocusNeighborRight, Side.Right)}\n" +
                $"Top:{NeighbourAsString(ctrl.FocusNeighborTop, Side.Top)}\n" +
                $"Bottom:{NeighbourAsString(ctrl.FocusNeighborBottom, Side.Bottom)}");
        }
    }
}
