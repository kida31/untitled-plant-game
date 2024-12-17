using Godot;

namespace untitledplantgame.GUI.Hooks;

public partial class FollowFocusedHook: Node
{
	private enum PositioningMode
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
	[Export] private PositioningMode _positionSelfMode = PositioningMode.Origin;
	/// <summary>
	/// <para>The node positioning behavior. Whether the origin or center position of focused object should be used.</para>
	/// </summary>
	[Export] private PositioningMode _attachmentMode = PositioningMode.Origin;
	[Export] private float _smoothingFactor = 1.0f;
	
	private Control _recentFocused = null;
	private Control _target;

	public override void _Ready()
	{
		_target = GetParent<Control>();
	}

	public override void _Process(double delta)
	{
		if (_recentFocused == null || !_recentFocused.HasFocus())
		{
			_recentFocused = null;
			return;
		}

		var newPos = CalculateFocusedPosition(_recentFocused, _attachmentMode);
		newPos = CalculateEffectivePosition(newPos, _positionSelfMode);

		if (_smoothingFactor > 0)
		{
			newPos = _target.GlobalPosition.Lerp(newPos, _smoothingFactor * (float) delta);
		}
		
		_target.GlobalPosition = newPos;
	}


	private Vector2 CalculateFocusedPosition(Control node, PositioningMode mode)
	{
		var rect = node.GetGlobalRect();
		return mode switch
		{
			PositioningMode.Origin => rect.Position,
			PositioningMode.Center => rect.Position + rect.Size / 2,
			_ => Vector2.Zero
		};
	}
	
	private Vector2 CalculateEffectivePosition(Vector2 position, PositioningMode mode)
	{
		return mode switch
		{
			PositioningMode.Origin => position,
			PositioningMode.Center => position - _target.GetGlobalRect().Size / 2,
			_ => Vector2.Zero
		};
	}
}
