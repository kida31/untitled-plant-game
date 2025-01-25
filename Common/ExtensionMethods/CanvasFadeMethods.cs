using Godot;

namespace untitledplantgame.Common.ExtensionMethods;

/// <summary>
///     Methods to fade in and out a CanvasItem.
/// </summary>
public static class CanvasFadeMethods
{
	/// <summary>
	///     Fades in the CanvasItem by tweening the alpha value of the modulate property.
	///     The duration is the time it takes to fade in.
	///     Returns the tween handler.
	/// </summary>
	/// <param name="self"></param>
	/// <param name="duration"></param>
	/// <param name="tween">The previous teen to append to</param>
	/// <returns></returns>
	public static Tween FadeIn(this CanvasItem self, float duration, Tween tween = null)
	{
		return FadeAlpha(self, duration, 1.0f, tween);
	}

	/// <summary>
	/// 	Fades out the CanvasItem by tweening the alpha value of the modulate property.
	///     The duration is the time it takes to fade out.
	///     Returns the tween handler.
	/// </summary>
	/// <param name="self"></param>
	/// <param name="duration"></param>
	/// <param name="tween">The previous teen to append to</param>
	/// <returns></returns>
	public static Tween FadeOut(this CanvasItem self, float duration, Tween tween = null)
	{
		return FadeAlpha(self, duration, 0.0f, tween);
	}

	private static Tween FadeAlpha(this CanvasItem self, float duration, float alpha, Tween tween)
	{
		tween ??= self.CreateTween();
		var modulate = self.Modulate;
		modulate.A = alpha;
		tween.TweenProperty(self, "modulate", modulate, duration);
		return tween;
	}
}
