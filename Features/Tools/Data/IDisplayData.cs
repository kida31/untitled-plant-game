using Godot;

namespace untitledplantgame.Tools.ToolDatas;

public interface IDisplayData
{
	public string Name { get; }

	/// <summary>
	///     Flavour text.
	/// </summary>
	public string Description { get; }

	public Texture2D Icon { get; }
}
