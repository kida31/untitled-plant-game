using Godot;
using System;
using System.Collections.Generic;

namespace untitledplantgame.Inventory.GUI;

public partial class TooltipView : PanelContainer
{
	private const bool AutomaticallyFreeOldContent = true;

	[Export] private Label _nameLabel;
	[Export] private Label _descriptionLabel;
	[Export] private Separator _separator;
	[Export] private Control _contentContainer;

	public string Title
	{
		get => _nameLabel.Text;
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

	private void SetTitle(string value)
	{
		_nameLabel.Text = value;
		if (string.IsNullOrEmpty(value))
		{
			_nameLabel.Hide();
			_separator.Hide();
		}
		else
		{
			_nameLabel.Show();
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
}
