#if TOOLS
using Godot;
using System;

namespace untitledplantgame.addons.upg_utils;

[Tool]
public partial class Plugin : EditorPlugin
{
	public const string RootProjectSettingsPath = "plugins/untitledplantgame_utils";
	
	private const string MainPanelPath = "res://addons/upg_utils/MainPanel.tscn";

	private Control _mainPanelInstance;

	public override void _EnterTree()
	{
		var mainPanel = ResourceLoader.Load<PackedScene>(MainPanelPath);
		_mainPanelInstance = (Control) mainPanel.Instantiate();
		// Add the main panel to the editor's main viewport.
		EditorInterface.Singleton.GetEditorMainScreen().AddChild(_mainPanelInstance);
		// Hide the main panel. Very much required.
		_MakeVisible(false);
	}

	public override void _ExitTree()
	{
		if (IsInstanceValid(_mainPanelInstance))
		{
			_mainPanelInstance.QueueFree();
		}
	}

	public override bool _HasMainScreen()
	{
		return true;
	}

	public override void _MakeVisible(bool visible)
	{
		if (IsInstanceValid(_mainPanelInstance))
		{
			_mainPanelInstance.Visible = visible;
		}
	}

	public override string _GetPluginName() => "UntitledPlantGame";

	public override Texture2D _GetPluginIcon()
	{
		return EditorInterface.Singleton.GetEditorTheme().GetIcon("Node", "EditorIcons");
	}
}
#endif
