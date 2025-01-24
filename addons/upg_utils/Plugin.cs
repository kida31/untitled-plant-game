#if TOOLS
using Godot;
using System;
using untitledplantgame.addons.upg_utils.Components;

namespace untitledplantgame.addons.upg_utils;

[Tool]
public partial class Plugin : EditorPlugin
{
	[Obsolete("Unused")]
	private const string MainPanelPath = "res://addons/upg_utils/MainPanel.tscn";
	private const string LogLevelDropdownPath = "res://addons/upg_utils/LogLevelDropdown.tscn";
	private const CustomControlContainer LogLevelDropdownContainer = CustomControlContainer.Toolbar;

	private Control _logLevelDropdown;

	public override void _EnterTree()
	{
		// Log Level Thingy in toolbar
		_logLevelDropdown = ResourceLoader.Load<PackedScene>(LogLevelDropdownPath).Instantiate<Control>();
		AddControlToContainer(LogLevelDropdownContainer, _logLevelDropdown);
		// Consider moving script to addon
		var autoScrollScript = GD.Load<Script>("res://Features/GUI/Components/Scrollable/AutoScrollRichTextLabel.cs");
		var icon = EditorInterface.Singleton.GetEditorTheme().GetIcon("RichTextLabel", "EditorIcons");
		AddCustomType("AutoScrollRichTextLabel", "RichTextLabel", autoScrollScript, icon);
	}

	public override void _ExitTree()
	{
		RemoveControlFromContainer(LogLevelDropdownContainer, _logLevelDropdown);
		_logLevelDropdown.QueueFree();
		RemoveCustomType("AutoScrollRichTextLabel");
	}
}
#endif
