using Godot;

namespace untitledplantgame.GUI.Traits;

public interface IFadable
{
	public void FadeIn(Control self, float duration)
	{
		var tween = self.CreateTween();
		var modulate = self.Modulate;
		modulate.A = 1.0f;
		tween.TweenProperty(self, "modulate", modulate, duration);
	}

	public void FadeOut(Control self, float duration)
	{
		var tween = self.CreateTween();
		var modulate = self.Modulate;
		modulate.A = 0.0f;
		tween.TweenProperty(self, "modulate", modulate, duration);
	}
}
