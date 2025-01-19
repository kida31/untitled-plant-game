using Godot;

namespace untitledplantgame.Scenes;

[Tool]
public partial class BlurEffect : ColorRect
{
	private const string LodParameter = "lod";
	private const float LodMax = 5.0f;
	private const float LodMin = 0.0f;

	[Export(PropertyHint.Range, "0.0,5.0")]
	public float Strength
	{
		get => GetStrength();
		set => SetStrength(value);
	}

	private float _strength = 0;

	public float GetStrength()
	{
		return _strength;
	}

	public void SetStrength(float value)
	{
		_strength = Mathf.Clamp(value, LodMin, LodMax);
		var material = Material as ShaderMaterial;
		material!.SetShaderParameter(LodParameter, _strength);
	}
}
