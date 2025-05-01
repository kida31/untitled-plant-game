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

	/// <summary>
	///		Switch to the next tool in the toolbelt.
	/// </summary>
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

	/// <summary>
	///		Switch to the previous tool in the toolbelt.
	/// </summary>
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
	
	public void AddTool(Tool tool)
	{
		if (tool == null)
		{
			_logger.Error("Tool is null, cannot add.");
			return;
		}
		if(HasTool(tool)) return;

		Array.Resize(ref _tools, _tools.Length + 1);
		_tools[^1] = tool;
		_toolIndex = _tools.Length - 1;
		if(_toolIndex < 0)
		{
			_logger.Error("Tool index is less than 0, this should not happen.");
		}
		
		_logger.Info($"Added tool: {tool}");
		WentToNextTool?.Invoke();
	}

	private bool HasTool(Tool tool)
	{
		return Tools.Contains(tool);
	}
}
