using Godot;

namespace untitledplantgame.GUI.Hooks;

/// <summary>
///     This hook makes parent Control node follow the cursor.
/// </summary>
public partial class FollowCursorHook : Node
{
	private enum AdjustPositionType
	{
		/// <summary>
		/// <para>Anchor the node to the cursor.</para>
		/// </summary>
		Anchor,

		/// <summary>
		/// <para>Center the node on the cursor.</para>
		/// </summary>
		Center
	}

	/// <summary>
	/// <para>The node positioning behavior (see <see cref="FollowCursor"/>).</para>
	/// </summary>
	[Export] private AdjustPositionType _mode = AdjustPositionType.Anchor;
	[Export] private float _smoothingFactor = 1.0f;

	private Vector2 _mousePosition = Vector2.Inf;
	private Control _target;


	public override void _Ready()
	{
		_target = GetParent<Control>();
	}

	public override void _Process(double delta)
	{
		if (_mousePosition == Vector2.Inf)
		{
			return;
		}

		var newPos = CalculateTargetPosition(_mousePosition);

		if (_smoothingFactor > 0)
		{
			newPos = _target.GlobalPosition.Lerp(newPos, _smoothingFactor * (float) delta);
		}
		
		_target.GlobalPosition = newPos;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_mousePosition = mouseMotion.Position;
		}
	}

	private Vector2 CalculateTargetPosition(Vector2 position)
	{
		return _mode switch
		{
			AdjustPositionType.Anchor => position,
			AdjustPositionType.Center => position - _target.GetGlobalRect().Size / 2,
			_ => Vector2.Zero
		};
	}
}
