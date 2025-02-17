using System;
using System.Collections.ObjectModel;
using untitledplantgame.Common;

namespace untitledplantgame.Tools;

/// <summary>
///		A container for a set of tools. Manages switching between tools and keeping track of the current tool.
/// </summary>
public class Toolbelt
{
	public event Action WentToNextTool;
	public event Action WentToPreviousTool;
	public event Action<Tool> ToolChanged;

	public Tool CurrentTool => _toolIndex < 0 ? null : _tools[_toolIndex];
	public Tool LeftTool => GetLeftToolOrNull();
	public Tool RightTool => GetRightToolOrNull();

	public ReadOnlyCollection<Tool> Tools => Array.AsReadOnly(_tools);

	private Tool[] _tools;
	private int _toolIndex;
	private readonly Logger _logger;

	public Toolbelt() : this(new Tool[] { })
	{
	}

	public Toolbelt(Tool[] tools)
	{
		_tools = tools;
		_toolIndex = tools.Length - 1; // -1 if it's empty
		_logger = new Logger("Toolbelt");
	}

	public void GoToNext()
	{
		// I feel like theres some smarter math to do here
		if ((_tools?.Length ?? 0) <= 0)
		{
			_toolIndex = -1;
		}
		else
		{
			_toolIndex = (_toolIndex + 1) % _tools.Length;
			_logger.Info("Switch to tool: " + CurrentTool);
			WentToNextTool?.Invoke();
			ToolChanged?.Invoke(CurrentTool);
		}
	}

	public void GoToPrevious()
	{
		// I feel like there's some smarter math to do here
		if ((_tools?.Length ?? 0) <= 0)
		{
			_toolIndex = -1;
		}
		else
		{
			_toolIndex = (_toolIndex + _tools.Length - 1) % _tools.Length;
			_logger.Info("Switch to tool: " + CurrentTool);
			WentToPreviousTool?.Invoke();
			ToolChanged?.Invoke(CurrentTool);
		}
	}

	private Tool GetLeftToolOrNull()
	{
		if (_toolIndex < 0)
		{
			return null;
		}

		return _toolIndex - 1 >= 0 ? _tools[_toolIndex - 1] : _tools[^1];
	}

	private Tool GetRightToolOrNull()
	{
		if (_toolIndex < 0)
		{
			return null;
		}

		return _toolIndex + 1 < _tools.Length ? _tools[_toolIndex + 1] : _tools[0];
	}
}
