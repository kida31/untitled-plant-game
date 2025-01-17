using Godot;

namespace untitledplantgame.Common.ExtensionMethods;

public static class FadingMethods
{
	public static Tween FadeIn(this CanvasItem self, float duration)
	{
		return FadeAlpha(self, duration, 1.0f);
	}

	public static Tween FadeOut(this CanvasItem self, float duration)
	{
		return FadeAlpha(self, duration, 0.0f);
	}

	private static Tween FadeAlpha(this CanvasItem self, float duration, float alpha)
	{
		var tween = self.CreateTween();
		var modulate = self.Modulate;
		modulate.A = alpha;
		tween.TweenProperty(self, "modulate", modulate, duration);
		return tween;
	}
}
