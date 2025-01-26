using Godot;

namespace untitledplantgame;

public partial class PixelViewport : SubViewportContainer
{
	public static PixelViewport Instance { get; private set; }
	
	private ShaderMaterial _shaderMaterial;

	public override void _Ready()
	{
		if (Instance != null)
		{
			GD.PrintErr("There should only be one PixelViewport in the scene");
			QueueFree();
		}

		Instance = this;
	}

	public void SetOffset(Vector2 offset)
	{
		const string parameterName = "cam_offset";
		var shader = GetMaterial() as ShaderMaterial;
		shader!.SetShaderParameter(parameterName, offset);
	}
}
