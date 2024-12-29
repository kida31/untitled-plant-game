using System;
using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Database;
using untitledplantgame.Inventory.GUI;
using untitledplantgame.Player;
using untitledplantgame.Shops;

namespace untitledplantgame.Inventory.PlayerInventory;

// TODO: Consider flattening the component tree

// Reminder: C# doesn't copy objects inside of lists, but rather creates an additional reference!
// - crisscrossmaster
public partial class BookView : Control
{
	[ExportGroup("Page References")]
	// Page references for updating content

	[Export] private PlayerInventoryPage _playerInventoryPage;
	[Export] private WikiPage _wikiPage;

	[ExportGroup("Tabs")]
	[Export] private TabContainer _tabContainer; // Maybe make bookview the tabcontainer itself
	[Obsolete("Unused. May be any class that has .Pressed event")]
	[Export] private Button[] _tabButtons = new Button[0];	
	// TODO add custom tab buttons

	public override void _Ready()
	{
		// Subscribe to events
		EventBus.Instance.OnPlayerInventoryChanged += UpdateInventory;
		EventBus.Instance.OnInventoryOpen += ShowBook;

		// Connect tab buttons to trigger tabs
		for (var index = 0; index < _tabButtons.Length; index++)
		{
			var button = _tabButtons[index];
			var capturedIndex = index;
			button.Pressed += () => _tabContainer.CurrentTab = capturedIndex;
		}
		
		// Inventory
		
		// Wiki
		var items = ItemDatabase.Instance.ItemStacks;
		_wikiPage.UpdateItems(items);
		
		_wikiPage.ItemStackPressed += item => _wikiPage.UpdateArticle(item);
	}



	public override void _UnhandledInput(InputEvent @event)
	{
		// I want to do If-return for readability because i think if-else reads like crap
		// But if some dimwit forgets the return it would cause some bugs

		// Open book
		
		if (@event.IsActionPressed(FreeRoam.OpenBook))
		{
			ShowBook();
			
		}
		
		// Close book
		
		else if (@event.IsAction(Book.Back) || @event.IsActionPressed(Book.CloseBook))
		{
			// TODO: The back button might have to do different things depending on context
			HideBook();
			
		}
		
		// Switch main tabs
		
		else if (@event.IsActionPressed(Book.TriggerRight))
		{
			if (Math.Abs(@event.GetActionStrength(Book.TriggerRight) - 1.0) > double.Epsilon)
			{
				// Triggers will fire release and press events for each little bit of the trigger
				return;
			}
			var nextOrFirstTabIndex = (_tabContainer.CurrentTab + 1) % _tabContainer.GetChildCount();
			_tabContainer.CurrentTab = nextOrFirstTabIndex;
		}
		else if (@event.IsActionPressed(Book.TriggerLeft))
		{
			if (Math.Abs(@event.GetActionStrength(Book.TriggerLeft) - 1.0) > double.Epsilon)
			{
				return;
			}
			var count = _tabContainer.GetChildCount();
			var prevOrLastTabIndex = (_tabContainer.CurrentTab - 1 + count) % count;
			_tabContainer.CurrentTab = prevOrLastTabIndex;
		}
	}

	private void UpdateInventory(Player.Player player, IInventory inventory)
	{
		_playerInventoryPage.UpdateInventories(player.Inventory.GetSubInventories());
	}

	private void ShowBook()
	{
		UpdateInventory(Game.Player, Game.Player.Inventory);
		Show();
		GameStateMachine.Instance.SetState(GameState.Book);
	}

	private void HideBook()
	{
		Hide();
		GameStateMachine.Instance.SetState(GameState.FreeRoam);
	}
}
