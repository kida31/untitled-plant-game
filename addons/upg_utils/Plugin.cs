#if TOOLS
using Godot;
using System;
using untitledplantgame.addons.upg_utils.Components;

namespace untitledplantgame.addons.upg_utils;

[Tool]
public partial class Plugin : EditorPlugin
{
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

		// Simple talking npc
		var simpleTalkingObjectScript = GD.Load<Script>("res://Features/NPC/NpcType/SimpleTalkingObject.cs");
		var simpleTalkingObjectIcon = EditorInterface.Singleton.GetEditorTheme().GetIcon("Area2D", "EditorIcons");
		AddCustomType("SimpleTalkingObject", "Area2D", simpleTalkingObjectScript, simpleTalkingObjectIcon);
	}

	public override void _ExitTree()
	{
		RemoveControlFromContainer(LogLevelDropdownContainer, _logLevelDropdown);
		_logLevelDropdown.QueueFree();
		RemoveCustomType("AutoScrollRichTextLabel");
	}
}
#endif
