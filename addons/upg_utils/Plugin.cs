#if TOOLS
using Godot;
using System;
using untitledplantgame.addons.upg_utils.Components;

namespace untitledplantgame.addons.upg_utils;

[Tool]
public partial class Plugin : EditorPlugin
{
	private const string MenuBar = "res://addons/upg_utils/MenuBar.tscn";
	private const CustomControlContainer MenuBarContainer = CustomControlContainer.Toolbar;

	private Control _menuBar;

	public override void _EnterTree()
	{
		// Log Level Thingy in toolbar
		_menuBar = ResourceLoader.Load<PackedScene>(MenuBar).Instantiate<Control>();
		AddControlToContainer(MenuBarContainer, _menuBar);

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
		RemoveControlFromContainer(MenuBarContainer, _menuBar);
		_menuBar.QueueFree();
		
		RemoveCustomType("AutoScrollRichTextLabel");
	}
}
#endif
