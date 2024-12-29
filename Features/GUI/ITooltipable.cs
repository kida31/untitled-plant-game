using Godot;

public interface ITooltipable
{
    string Title { get; }
    string Description { get; }
    Control Content { get; }
}