using System;
using System.Collections.Generic;
using Godot;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
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
	[Export] private Button[] _tabButtons;	
	// TODO add custom tab buttons

	public override void _Ready()
	{
		// Subscribe to events
		EventBus.Instance.OnItemPickUp += UpdateInventory;
		EventBus.Instance.OnInventoryOpen += ShowBook;

		// Connect tab buttons to trigger tabs
		for (var index = 0; index < _tabButtons.Length; index++)
		{
			var button = _tabButtons[index];
			var capturedIndex = index;
			button.Pressed += () => _tabContainer.CurrentTab = capturedIndex;
		}


		// TODO move to controller/presenter
		// Inventory
		var rand = new RandomStockGenerator();
		var a = new Inventory(15, "A");
		a.SetContents(rand.GetRandom(12));
		var b = new Inventory(15, "A");
		b.SetContents(rand.GetRandom(5));
		var c = new Inventory(15, "A");
		c.SetContents(rand.GetRandom(8));
		_playerInventoryPage.SetInventories(new ()
		{
			a,
			b,
			c,
		});

		// Wiki
		_wikiPage.ItemStackPressed += item => _wikiPage.UpdateArticle(item);
		_wikiPage.UpdateItems(rand.GetRandom(12));
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// I want to do If-return for readability because i think if-else reads like crap
		// But if some dimwit forgets the return it would cause some bugs

		if (@event.IsActionPressed(FreeRoam.OpenBook))
		{
			Visible = true;
			GameStateMachine.Instance.SetState(GameState.Book);
		}
		else if (@event.IsAction(Book.Back))
		{
			// TODO: This button might have to do different things depending on context
			Visible = false;
			GameStateMachine.Instance.SetState(GameState.FreeRoam);
		}
		else if (@event.IsActionPressed(Book.CloseBook))
		{
			Visible = false;
			GameStateMachine.Instance.SetState(GameState.FreeRoam);
		}
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

	private void UpdateInventory(ItemStack item)
	{
		// TODO
		// EventBus.Instance.TabsUpdated(item);
	}

	private void ShowBook()
	{
		Show();
	}
}
