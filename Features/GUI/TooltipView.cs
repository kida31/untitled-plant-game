using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI;

/// <summary>
///     This control provides a tooltip-like view with a title, description and custom content.
/// </summary>
public partial class TooltipView : Control
{
	private const bool AutomaticallyFreeOldContent = true;
	private static float MaxWidth = 200; // in pixels

	public event Action<string> TitleChanged;
	public event Action<string> DescriptionChanged;

	[Export] private RichTextLabel _titleLabel;
	[Export] private RichTextLabel _descriptionLabel;
	[Export] private Separator _separator;
	[Export] private Control _contentContainer;
	[Export] private Label _referenceLabel;

	public string Title
	{
		get => _titleLabel.Text;
		set => SetTitle(value);
	}

	public string Description
	{
		get => _descriptionLabel.Text;
		set => SetDescription(value);
	}

	[Unstable("Not thoroughly tested")]
	public Control CustomContent
	{
		get => _customContent;
		set => SetCustomContent(value);
	}

	private Control _customContent;

	public override void _Ready()
	{
		TitleChanged += (_) => UpdateSeparator();
		DescriptionChanged += (_) => UpdateSeparator();
		
		// We adjust the title width according to a reference label.
		// This is necessary because RichTextLabel does not work like label.
		// FitContent SUCKS.
		// IT'S A LIE.
		// IT SAYS IT WILL BEHAVE LIKE LABEL BUT IT DOESN'T.
		// IT SIMPLY GROWS VERTICALLY AS NEEDED BUT DOES NOT SHRINK OR EXPAND HORIZONTALLY.
		_referenceLabel.ItemRectChanged += AutoAdjustWidth;
		MinimumSizeChanged += () => Size = Vector2.Zero; // Force resize in frame after change
		void UpdateReference(object ignored) => _referenceLabel.Text = FilterBBCode(Title + "\n" + Description);
		DescriptionChanged += UpdateReference;
		TitleChanged += UpdateReference;
		// EndShittyLabelAdjustment
	}

	public override void _ExitTree()
	{
		// Clean up
		_referenceLabel.ItemRectChanged -= AutoAdjustWidth;
	}

	private void SetTitle(string value)
	{
		value ??= "";
		_titleLabel.Text = value;
		TitleChanged?.Invoke(value);
	}

	private void SetDescription(string value)
	{
		value ??= "";
		_descriptionLabel.Text = value;
		DescriptionChanged?.Invoke(value);
	}

	/// <summary>
	///  Sets the custom content of the tooltip. This will add the provided nodes to the content container.
	/// <remarks>
	/// Previously used content will be freed if AutomaticallyFreeOldContent is set to true.
	/// </remarks>
	/// </summary>
	/// <param name="content"></param>
	private void SetCustomContent(Control content)
	{
		_customContent?.QueueFree();

		_customContent = content?.Duplicate() as Control;

		if (_customContent != null)
		{
			_contentContainer.AddChild(_customContent);
		}
	}

	/// <summary>
	/// Adjusts the width of the TitleLabel according to the reference label.
	/// Tooltip should auto resize according to the content.
	/// </summary>
	private void AutoAdjustWidth()
	{
		var newWidth = Math.Min(_referenceLabel.GetRect().Size.X, MaxWidth);
		_titleLabel.CustomMinimumSize = new Vector2(newWidth, 0);
		SetDeferred(Control.PropertyName.Size, Vector2.Zero);
	}

	/// <summary>
	/// Removes all bbcode blocks
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	private static string FilterBBCode(string text)
	{
		if (text == null)
		{
			return null;
		}

		return Regex.Replace(text, @"\[\/?(?:b|i|u|url|quote|code|img|color|size)*?.*?\]", "");
	}

	private void UpdateSeparator()
	{
		if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Description))
		{
			_separator.Hide();
		}
		else
		{
			_separator.Show();
		}
	}
}
