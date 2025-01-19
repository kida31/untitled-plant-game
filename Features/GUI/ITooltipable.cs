using Godot;

/// <summary>
///     Interface for nodes that can provide a tooltip.
///     Used by <see cref="untitledplantgame.GUI.GlobalTooltip" /> to display a
/// </summary>
public interface ITooltipable
{
	string Title { get; }
	string Description { get; }
	Control Content { get; }
}
