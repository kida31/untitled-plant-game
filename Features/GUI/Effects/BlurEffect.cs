using Godot;

namespace untitledplantgame.Scenes;

[Tool]
public partial class BlurEffect : Control
{
	private const string LodParameter = "lod";
	private const float LodMax = 5.0f;
	private const float LodMin = 0.0f;
	
	[Export(PropertyHint.Range, "0.0,5.0")] private float Strength
	{
		get => GetStrength();
		set => SetStrength(value);
	}

	[Export(PropertyHint.Range,"0.0,2.0")] private float _transitionDuration;
	[ExportGroup("Setup")] [Export] private CanvasItem _canvasItem;

	private float _strength;
	private float _currentLod;
	private Tween _tween; // Tween for strength transition
	
	public float GetStrength()
	{
		return _strength;
	}

	public void SetStrength(float value)
	{
		_strength = Mathf.Clamp(value, LodMin, LodMax);

		_tween?.Stop();
		_tween = CreateTween();
		_tween.TweenMethod(Callable.From<float>(SetLod), _currentLod, _strength, _transitionDuration);
	}

	private void SetLod(float value)
	{
		var material = _canvasItem.Material as ShaderMaterial;
		_currentLod = value;
		material!.SetShaderParameter(LodParameter, value);
	}
}
