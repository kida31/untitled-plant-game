using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using untitledplantgame.Common;

public partial class ResourcesHUD : MarginContainer
{
	private const double RoundingThreshold = 0.01f;

	private class ResourceRow
	{
		public RichTextLabel ValueLabel;
		public TextureRect IconRect;
		public Action<double> SetValue;
		public double CurrentValue;
	}

	[Export] private GridContainer _gridContainer;

	[Export] private Texture2D _coinIcon;

	private Dictionary<ResourceRow, double> _targetValues;

	public override void _Ready()
	{
		// Delete placeholders
		var placeholders = _gridContainer.GetChildren();
		foreach (var placeholder in placeholders)
		{
			_gridContainer.RemoveChild(placeholder);
			placeholder.QueueFree();
		}
		// END Delete placeholders

		_targetValues = new Dictionary<ResourceRow, double>();


		// TODO: Move this to parent?
		string FormatGold(double gold)
		{
			return gold switch
			{
				> 1_000_000_000 => gold.ToString("0,,,.##B"),
				> 1_000_000 => gold.ToString("0,,.##M"),
				// > 100_000 => gold.ToString("0,.##K"),
				_ => gold.ToString("N0", new NumberFormatInfo {NumberGroupSeparator = ""})
			};
		}

		AddRow(icon: _coinIcon, valueFormatter: FormatGold);
		EventBus.Instance.GoldChanged += (_, newGold) => _targetValues[_targetValues.Keys.First()] = newGold;
	}

	// TODO: Remove
	private int _demoGold = 0;

	// TODO: Remove stuff
	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_up"))
		{
			var g = (int) (1 + _demoGold * 0.5);
			_demoGold += g;
			EventBus.Instance.InvokeGoldChanged(g, _demoGold);
		}

		if (Input.IsActionJustPressed("ui_down"))
		{
			var g = (int) (-1 - 0.5f * _demoGold);
			_demoGold += g;
			_demoGold = Math.Max(0, _demoGold);
			EventBus.Instance.InvokeGoldChanged(g, _demoGold);
		}
	}

	public override void _Process(double delta)
	{
		foreach (var (row, targetValue) in _targetValues)
		{
			var diff = targetValue - row.CurrentValue;
			if (Math.Abs(diff) < RoundingThreshold)
			{
				continue;
			}

			// var d = row.CurrentValue + Math.Clamp(diff, -500 * delta, 100 * delta); // Constant
			var d = Mathf.Lerp(row.CurrentValue, targetValue, delta * 5); // Lerp
			row.SetValue(d);
		}
	}

	/// <summary>
	/// Adds a new line to the grid container with an icon and a value label.
	/// </summary>
	/// <param name="icon"></param>
	/// <param name="valueFormatter"></param>
	/// <param name="suffix"></param>
	/// <returns>The row object containing label, icon and update handler.</returns>
	private ResourceRow AddRow(Texture2D icon = null, Func<double, string> valueFormatter = null, string suffix = "")
	{
		var iconRect = new TextureRect
		{
			Texture = icon,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
			SizeFlagsVertical = SizeFlags.ShrinkCenter
		};

		var valueLabel = new RichTextLabel()
		{
			BbcodeEnabled = true,
			FitContent = true,
			AutowrapMode = TextServer.AutowrapMode.Off,
			GrowHorizontal = GrowDirection.Begin,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
		};

		var row = new ResourceRow()
		{
			ValueLabel = valueLabel,
			IconRect = iconRect,
			CurrentValue = 0,
		};

		void SetValue(double value)
		{
			row.CurrentValue = value;
			valueLabel.Text =
				$"[right]{valueFormatter?.Invoke(row.CurrentValue) ?? row.CurrentValue.ToString(CultureInfo.InvariantCulture)}{suffix}[/right]";
		}

		row.SetValue = SetValue;
		SetValue(0);

		_targetValues.Add(row, 0);
		_gridContainer.AddChild(row.IconRect);
		_gridContainer.AddChild(row.ValueLabel);
		return row;
	}

	private void RemoveRow(ResourceRow row)
	{
		_gridContainer.RemoveChild(row.ValueLabel);
		_gridContainer.RemoveChild(row.IconRect);
		_targetValues.Remove(row);
	}

	private void RemoveAllRows()
	{
		foreach (var row in _targetValues.Keys)
		{
			RemoveRow(row);
		}
	}
}
