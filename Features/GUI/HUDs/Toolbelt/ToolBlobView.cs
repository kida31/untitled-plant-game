using Godot;
using System;

[Tool]
public partial class ToolBlobView : MarginContainer
{
	public enum Style{
		Primary,
		Secondary
	}

	[Export] private Control _mainBg;
	[Export] private Control _secondaryBg;
	
	[Export] public Style BlobStyle
	{
		get => _style;
		set => SetBlobStyle(value);
	}
	
	private Style _style = Style.Primary;
	
	private void SetBlobStyle(Style value)
	{
		_style = value;
		_mainBg.Visible = _style == Style.Primary;
		_secondaryBg.Visible = _style == Style.Secondary;
	}
}
