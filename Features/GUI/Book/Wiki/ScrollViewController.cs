using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
/// Manages the scrolling state of the list of items in the wiki.
/// </summary>
public partial class ScrollViewController : ScrollContainer
{
	[Export] private VBoxContainer _scrollViewItems;

	private bool _isRunning;
	private float _distanceToFocusedElement;
	private double _speedCoefficient; // Speed and time calculation fall apart at lower speeds. An additional speed boost is needed
	private const int JumpToThreshold = 10; // Needs to be higher if scrolling should happen faster
	private const int ScrollToFocusedElementSpeed = 300; // Determines how long each automated scroll should be
	private WikiItemView _currentlySelectedWikiItemView;

	public override void _Ready()
	{
		GetAllScrollViewItems().First().GrabFocus();
		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
	}

	/// <summary>
	/// If selected node belongs to a wiki item view, that item view will be scrolled to.
	/// Remark: This is a blasphemous solution. I hate it. But it works.
	/// </summary>
	/// <param name="node"></param>
	private void OnGuiFocusChanged(Control node)
	{
		var itemView = GetFirstAncestor<WikiItemView>(node);
		if (itemView == null)
		{
			return;
		}

		ScrollToFocusedElement(itemView);
	}

	/// <summary>
	/// Gets the first ancestor of type. Searches at most maxDepth steps up the tree.
	/// </summary>
	/// <param name="child"></param>
	/// <param name="maxDepth"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	private T GetFirstAncestor<T>(Node child, int maxDepth = 5) where T : class
	{
		if (maxDepth <= 0)
		{
			return null;
		}

		var parent = child.GetParent();
		if (parent == null)
		{
			return null;
		}

		if (parent is T parentOfType)
		{
			return parentOfType;
		}

		return GetFirstAncestor<T>(parent, maxDepth - 1);
	}

	public override void _Process(double delta)
	{
		if (_isRunning)
		{
			var scrollDistance = _distanceToFocusedElement / ScrollToFocusedElementSpeed * delta * 1000 * _speedCoefficient;
			if (scrollDistance is <= 0 and >= -5) // special case for element directly above currently focused one
			{
				scrollDistance = -5;
			}

			ScrollVertical += (int) scrollDistance;

			if (Math.Abs(_currentlySelectedWikiItemView.Position.Y - ScrollVertical) <= JumpToThreshold)
			{
				ScrollToY(_currentlySelectedWikiItemView.Position.Y);
				_isRunning = false;
			}
		}
	}

	// Cancel the auto scrolling if player decides to intervene
	public override void _UnhandledInput(InputEvent @event)
	{
		// Delete after MVP
		// Are you sure?
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.WheelUp || mouseEvent.ButtonIndex == MouseButton.WheelDown)
			{
				_isRunning = false;
			}
		}
	}

	private async void StartTimedAction(WikiItemView wikiItemView)
	{
		_currentlySelectedWikiItemView = wikiItemView;
		_distanceToFocusedElement = wikiItemView.Position.Y - ScrollVertical;
		_speedCoefficient = Math.Pow(1.1, ScrollToFocusedElementSpeed / _distanceToFocusedElement);

		_isRunning = true;
		await Task.Delay(ScrollToFocusedElementSpeed);
	}

	private void ScrollToFocusedElement(WikiItemView wikiItemView)
	{
		if (_isRunning)
		{
			_isRunning = false;
		}

		StartTimedAction(wikiItemView);
	}

	private void ScrollToY(float elementY)
	{
		double scrollValue = Mathf.Clamp(elementY, 0, GetVScrollBar().MaxValue);

		ScrollVertical = (int) scrollValue;
	}

	private Array<WikiItemView> GetAllScrollViewItems()
	{
		var scrollViewItems = new Array<WikiItemView>();

		foreach (var node in _scrollViewItems.GetChildren())
		{
			if (node is WikiItemView scrollViewItem)
			{
				scrollViewItems.Add(scrollViewItem);
			}
			else
			{
				GD.PrintErr("The element: " + node + " was added to the scrollView, which is not supported!");
			}
		}

		return scrollViewItems;
	}
}
