using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace untitledplantgame.Inventory.GUI;

[Tool]
public partial class TooltipView : PanelContainer
{
	private const bool AutomaticallyFreeOldContent = true;
	private static float MaxWidth = 400f;

	[Export] private RichTextLabel _titleLabel;
	[Export] private RichTextLabel _descriptionLabel;
	[Export] private Separator _separator;
	[Export] private Control _contentContainer;
	[Export] private Label _referenceLabel;

	[Export]
	private string _exampleTitle
	{
		get => Title;
		set => Title = value;
	}

	[Export]
	private string _exampleDescription
	{
		get => Description;
		set => Description = value;
	}

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

	public List<Control> CustomContent
	{
		get => _customContent;
		set => SetCustomContent(value);
	}

	private List<Control> _customContent;

	public override void _Ready()
	{
		// We adjust the title width according to a reference label.
		// This is necessary because RichTextLabel does not work like label.
		// FitContent SUCKS.
		// IT'S A LIE.
		// IT SAYS IT WILL BEHAVE LIKE LABEL BUT IT DOESN'T.
		// IT SIMPLY GROWS VERTICALLY AS NEEDED BUT DOES NOT SHRINK OR EXPAND HORIZONTALLY.
		_referenceLabel.ItemRectChanged += AutoAdjustWidth;
	}


	private void SetTitle(string value)
	{
		_titleLabel.Text = value;
		_referenceLabel.Text = FilterBBCode(_titleLabel.Text);

		if (string.IsNullOrEmpty(value))
		{
			_titleLabel.Hide();
			_separator.Hide();
		}
		else
		{
			_titleLabel.Show();
			_separator.Show();
		}
	}

	private void SetDescription(string value)
	{
		_descriptionLabel.Text = value;
	}

	/// <summary>
	///  Sets the custom content of the tooltip. This will add the provided nodes to the content container.
	/// <remarks>
	/// Previously used content will be freed if AutomaticallyFreeOldContent is set to true.
	/// </remarks>
	/// </summary>
	/// <param name="content"></param>
	private void SetCustomContent(List<Control> content)
	{
		if (_customContent != null)
		{
			foreach (var node in _customContent)
			{
				_contentContainer.RemoveChild(node);
				if (AutomaticallyFreeOldContent)
				{
					node.QueueFree();
				}
			}
		}

		_customContent = content;

		if (_customContent != null)
		{
			foreach (var node in _customContent)
			{
				_contentContainer.AddChild(node);
			}
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
}
