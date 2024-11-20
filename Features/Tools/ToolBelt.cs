using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Tools;

namespace untitledplantgame.Tools;

public class ToolBelt
{
	public Tool ActiveTool { get; private set; }
	public Tool[] Tools => _tools.Values.ToArray();

	private SortedDictionary<Type, Tool> _tools = new();
	private Logger _logger = new("ToolkitComponent");

	/// <summary>
	///   Sets the active tool to the specified type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public void SetActiveTool<T>()
		where T : Tool
	{
		if (_tools.ContainsKey(typeof(T)))
		{
			ActiveTool = _tools[typeof(T)];
		}
	}

	/// <summary>
	///   Sets the tool of type T to the specified tool.
	/// </summary>
	/// <param name="tools"></param>
	public void SetTool(params Tool[] tools)
	{
		foreach (var tool in tools)
		{
			SetTool_(tool);
		}
	}

	
	/// <summary>
	///   Sets the tool of type T to the specified tool.
	/// </summary>
	/// <param name="tool"></param>
	/// <typeparam name="T"></typeparam>
	// This is private, but arranged close to its "overload" SetTool
	private void SetTool_<T>(T tool)
		where T : Tool
	{
		_logger.Debug($"Setting tool: {tool.GetType().Name}");

		// What's the difference?
		// _tools[tool.GetType()] = tool;
		_tools[typeof(T)] = tool;
	}

	/// <summary>
	///    Removes the tool of type T from the toolkit.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public void RemoveTool<T>()
		where T : Tool
	{
		_logger.Debug($"Removing tool: {typeof(T).Name}");
		_tools.Remove(typeof(T));
	}

	/// <summary>
	///    Returns the next tool in the toolkit and changes the active tool to that tool.
	/// </summary>
	/// <returns></returns>
	public Tool Next()
	{
		var currentToolIndex = Array.IndexOf(Tools, ActiveTool);
		var nextToolIndex = currentToolIndex + 1;
		if (nextToolIndex >= Tools.Length)
		{
			nextToolIndex = 0;
		}
		ActiveTool = Tools[nextToolIndex];
		return ActiveTool;
	}

	/// <summary>
	///   Returns the previous tool in the toolkit and changes the active tool to that tool.
	/// </summary>
	/// <returns></returns>
	public Tool Previous()
	{
		var currentToolIndex = Array.IndexOf(Tools, ActiveTool);
		var previousToolIndex = currentToolIndex - 1;
		if (previousToolIndex < 0)
		{
			previousToolIndex = Tools.Length - 1;
		}
		ActiveTool = Tools[previousToolIndex];
		return ActiveTool;
	}
}
