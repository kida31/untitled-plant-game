using Godot;
using System;
using System.Linq;

/// <summary>
/// This node sequentially fills all progressbar children elements
/// </summary>
[Tool]
public partial class FaithProgressBar : Control
{
	/// <summary>
	/// Value for ProgressBar. 0 to X.
	/// </summary>
	[Export]
	public double Value
	{
		get => _value;
		set {
			_value = value;
			OnValueSet();
		}
	}

	/// <summary>
	/// Remaps value to [0, 1] instead of using 0 to N (N being number of sub progress bars)
	/// </summary>
	[Export]
	public bool RemapValue = false;

	private double _value = 0.0;

	private TextureProgressBar[] GetProgressBars()
	{
		return GetChildren().OfType<TextureProgressBar>().ToArray();
	}

	private void OnValueSet()
	{
		var progressBars = GetProgressBars();
		var eValue = RemapValue ? _value * progressBars.Length : _value; // Use [0, 1] if mapped, else [0, Max]

		for (int i = 0; i < progressBars.Length; i++)
		{
			var bar = progressBars[i];
			if (i < (int) eValue) {
				bar.Value = bar.MaxValue;
			} else {
				bar.Value = (eValue - i) * bar.MaxValue;
			}
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		return GetChildren()
			.Where(c => c is not TextureProgressBar)
			.Select(c => $"'{c.Name}' is not a TextureProgressBar and will be ignored")
			.ToArray();
	}

}
