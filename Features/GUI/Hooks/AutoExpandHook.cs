using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.Hooks;

/// <summary>
///		Add this node as a child of a Control node to make it expand in a way that keeps the aspect ratio
///		One dimension is chosen as primary dimension. The other dimension is then adjusted to keep the aspect ratio.
///		Aspect ratio can be manually set initially or will be calculated from the size of the parent node in _Ready.
/// </summary>
[Tool]
public partial class AutoExpandHook : Node
{
	enum Mode
	{
		/// <summary>
		/// Tries to keep aspect ratio when the height changes
		/// </summary>
		WidthMatchHeight,

		/// <summary>
		/// Tries to keep aspect ratio when the width changes
		/// </summary>
		HeightMatchWidth,
	}

	[Export] private float _aspectRatio = 0.0f;
	[Export] private Mode _mode = Mode.HeightMatchWidth;

	private Control _parent;
	private Logger _logger;

	public override void _EnterTree()
	{
		_parent = GetParent<Control>();
		_logger = new Logger(this);
		if (_aspectRatio <= double.Epsilon)
		{
			var size = _parent.GetGlobalRect().Size;
			_aspectRatio = size.X / size.Y;
		}

		_parent.ItemRectChanged += OnItemRectChanged;
	}

	public override void _ExitTree()
	{
		// Clean up

		var callable = Callable.From(OnItemRectChanged);
		var isConnected = _parent.IsConnected(CanvasItem.SignalName.ItemRectChanged, callable);
		// Bad Practice Catch(anything) - This is a workaround for a (I assume) Godot bug
		if (!isConnected)
		{
			// Why is it possible that the event is not added? It's in _Ready/_EnterTree
			// [Tool] scripts are isolated from the rest of the scene tree.
			// Funky stuff can happen when reloading scripts while not reloading scenes/editor

			// C# sharp can unregister delegates without having connected earlier, no?
			// This seems to be a Godot specific error.
			// C# usually allows to remove a delegate from an event even if it is not added.
			// Godot Signals do not allow that.

			// This is probably editor-only and only during development (recompiling scripts)
			_logger?.Error("OnItemRectChanged was not connected to ItemRectChanged signal. Disconnect suppressed.");
			return;
		}

		_parent.ItemRectChanged -= OnItemRectChanged;
	}

	private void OnItemRectChanged()
	{
		var size = _parent.GetGlobalRect().Size;
		var newMinSize = _parent.CustomMinimumSize;
		if (_mode == Mode.HeightMatchWidth)
		{
			newMinSize.Y = size.X / _aspectRatio;
			_parent.CustomMinimumSize = newMinSize;
		}
		else
		{
			newMinSize.X = size.Y * _aspectRatio;
			_parent.CustomMinimumSize = newMinSize;
		}
	}
}
