using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class ScrollViewController : ScrollContainer
{

	[Export] private Button _plantButton;
	[Export] private Button _materialButton;
	[Export] private Button _medicineButton;
	[Export] private VBoxContainer _scrollViewItems;

	private bool _isRunning;
	private float _distanceToFocusedElement;
	private double _speedCoefficient; // Speed and time calculation fall apart at lower speeds. An additional speed boost is needed
	private const int JumpToThreshold = 10; // Needs to be higher if scrolling should happen faster
	private const int ScrollToFocusedElementSpeed = 300; // Determines how long each automated scroll should be
	private WikiItemView _currentlySelectedWikiItemView;
	private System.Collections.Generic.Dictionary<Button, Action> _buttonActions;
	
	public override void _Ready()
	{
		GetAllScrollViewItems().First().GrabFocusToButton();
		
		EventBus.Instance.OnScrollContainerViewUpdate += ScrollToFocusedElement;
		
		_buttonActions = new System.Collections.Generic.Dictionary<Button, Action>
		{
			{ _plantButton, ScrollToFirstPlantElement },
			{ _materialButton, ScrollToFirstMaterialElement },
			{ _medicineButton, ScrollToFirstMedicineElement }
		};
		
		foreach (var pair in _buttonActions)
		{
			pair.Key.Pressed += () => pair.Value();
		}
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
		_speedCoefficient = Math.Pow(1.1, ScrollToFocusedElementSpeed/_distanceToFocusedElement); 
		
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

		ScrollVertical = (int)scrollValue;
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
	
	private void ScrollToFirstPlantElement()
	{
		foreach (var controlNode in GetAllScrollViewItems())
		{
			if (controlNode is WikiItemView wikiItemView)
			{
				if (wikiItemView.ItemStack.Category == ItemCategory.Plant)
				{
					//ScrollToY(controlNode.Position.Y);
					ScrollToY(0);
					return;
				}
			}
		}
	}

	private void ScrollToFirstMaterialElement()
	{
		foreach (var controlNode in GetAllScrollViewItems())
		{
			if (controlNode is WikiItemView wikiItemView)
			{
				if (wikiItemView.ItemStack.Category == ItemCategory.Plant) // Currently no Material Item
				{
					//ScrollToY(controlNode.Position.Y);
					ScrollToY(164);
					return;
				}
			}
		}
	}

	private void ScrollToFirstMedicineElement()
	{
		foreach (var controlNode in GetAllScrollViewItems())
		{
			if (controlNode is WikiItemView wikiItemView)
			{
				if (wikiItemView.ItemStack.Category == ItemCategory.Plant) // Currently no Medicine Item
				{
					//ScrollToY(controlNode.Position.Y);
					ScrollToY(328);
					return;
				}
			}
		}
	}
}
