using System;
using Godot;
using untitledplantgame.Tools;

namespace untitledplantgame.GUI.Book.Inventories;

public partial class ToolCircle : TextureRect
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
}
