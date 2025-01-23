using System;
using Godot;
using untitledplantgame.Tools;

namespace untitledplantgame.GUI.Book.Inventories;

public partial class ToolCircle : TextureRect, ITooltipable
{
	[Export] protected TextureRect _toolImage;
	protected Tool _tool;

	public virtual Tool Tool
	{
		get => _tool;
		set
		{
			_tool = value;
			_toolImage.Texture = _tool?.Icon;
		}
	}

	public string Title => _tool?.Name;
	public string Description => _tool?.Description;
	public Control Content { get; }
}
