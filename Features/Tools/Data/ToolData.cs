using Godot;

namespace untitledplantgame.Tools.ToolDatas;

[GlobalClass]
public partial class ToolData : Resource, IToolUseData, IDisplayData
{
	[ExportCategory("Display")] [Export] public string Name { get; set; }
	[Export] public string Description { get; set; }
	[Export] public Texture2D Icon { get; set; }

	[ExportCategory("Usage")] [Export] public float ChannelingTime { get; set; }
	[Export] public float Radius { get; set; }
	[Export] public float Range { get; set; }
}
