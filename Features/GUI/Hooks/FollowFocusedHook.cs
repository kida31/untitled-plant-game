using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.Hooks;

/// <summary>
///		This hook makes parent Control node follow the focused control node.
/// </summary>
public partial class FollowFocusedHook : Node
{
	public enum PositionMode
	{
		/// <summary>
		/// <para>Anchor the node to the cursor.</para>
		/// </summary>
		Origin,

		/// <summary>
		/// <para>Center the node on the cursor.</para>
		/// </summary>
		Center
	}

	/// <summary>
	/// <para>The node positioning behavior. Whether the origin or center position of target should be moved.</para>
	/// </summary>
	[Export] public PositionMode PositionModeSelf = PositionMode.Center;
	/// <summary>
	/// <para>The node positioning behavior. Whether the origin or center position of focused object should be used.</para>
	/// </summary>
	[Export] public PositionMode PositionModeTarget = PositionMode.Center;
	/// <summary>
	/// <para>The smoothing factor for the position change. Values small or equal to 0 make changes instant</para>
	/// </summary>
	[Export] public float SmoothingFactor = 10.0f;

	private Control _recentFocused;
	private Control _self;
	private Logger _logger;

	public override void _Ready()
	{
		_self = GetParent<Control>();
		_recentFocused = null;
		_logger = new Logger(this);

		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
	}

	private void OnGuiFocusChanged(Control node)
	{
		if (!IsInstanceValid(node))
		{
			_recentFocused = null;
			return;
		}
		
		if (_recentFocused == null)
		{
			// First time focus
			_recentFocused = node;
			ForcePositionUpdate();
		}
		else
		{
			// Update focus
			_recentFocused = node;
		}
	}

	public override void _Process(double delta)
	{
		if (_recentFocused == null || !IsInstanceValid(_recentFocused)) return;
		if (!_recentFocused.HasFocus())
		{
			_recentFocused = null;
			_logger.Debug("Focus lost. Stop tracking.");
			return;
		}

		var position = CalcFocusedElementPosition(_recentFocused);
		position = AddOriginOffset(position);
		position = SmoothenPosition(position, delta);
		_self.GlobalPosition = position;
	}

	private void ForcePositionUpdate()
	{
		var position = CalcFocusedElementPosition(_recentFocused);
		position = AddOriginOffset(position);
		_self.GlobalPosition = position;
	}

	private Vector2 CalcFocusedElementPosition(Control node)
	{
		var rect = node.GetGlobalRect();
		return PositionModeTarget switch
		{
			PositionMode.Origin => rect.Position,
			PositionMode.Center => rect.Position + rect.Size / 2,
			_ => Vector2.Zero
		};
	}

	private Vector2 AddOriginOffset(Vector2 position)
	{
		return PositionModeSelf switch
		{
			PositionMode.Origin => position,
			PositionMode.Center => position - _self.GetGlobalRect().Size / 2,
			_ => Vector2.Zero
		};
	}

	private Vector2 SmoothenPosition(Vector2 pos, double delta)
	{
		if (SmoothingFactor <= 0) return pos;
		return _self.GlobalPosition.Lerp(pos, SmoothingFactor * (float) delta);
	}
}
