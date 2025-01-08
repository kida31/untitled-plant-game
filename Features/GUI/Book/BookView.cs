using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Database;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory;
using untitledplantgame.Player;

namespace untitledplantgame.GUI.Book;

// TODO: Consider flattening the component tree

// Reminder: C# doesn't copy objects inside of lists, but rather creates an additional reference!
// - crisscrossmaster
/// <summary>
/// This class represents the book view, which is a GUI element that displays the player's inventory and a wiki.
/// Functionally it is a tab container with two tabs: Inventory and Wiki.
/// This class is responsible for updating the content of the tabs and switching between them.
/// </summary>
public partial class BookView : Control
{
	[ExportGroup("Page References")]
	// Page references for updating content
	[Export]
	private PlayerInventoryPage _playerInventoryPage;

	[Export] private WikiPage _wikiPage;

	[ExportGroup("Tabs")] [Export] private TabContainer _tabContainer; // Maybe make bookview the tabcontainer itself
	[Obsolete("Unused. May be any class that has .Pressed event")] [Export] private Button[] _tabButtons = new Button[0];

	private Logger _logger;

	public override void _Ready()
	{
		// 1. Init self
		_logger = new(this);
		
		// 1.1. Subscribe to events
		EventBus.Instance.OnPlayerInventoryChanged += UpdateInventory;
		EventBus.Instance.OnInventoryOpen += ShowBook;

		// TODO: UNUSED. We currently only use trigger buttons to switch tabs
		// 1.2 Connect tab buttons to trigger tabs
		// for (var index = 0; index < _tabButtons.Length; index++)
		// {
		// 	var button = _tabButtons[index];
		// 	var capturedIndex = index;
		// 	button.Pressed += () => _tabContainer.CurrentTab = capturedIndex;
		// }

		// 2. Init Inventory

		// 3. Init Wiki
		var items = ItemDatabase.Instance.ItemStacks;
		_wikiPage.UpdateItems(items);
		_wikiPage.ItemStackPressed += item => _wikiPage.UpdateArticle(item);
	}


	public override void _UnhandledInput(InputEvent @event)
	{
		// Book is closed -> Open book
		if (@event.IsActionPressed(FreeRoam.OpenBook))
		{
			ShowBook();
			return;
		}
		
		// Book is open
		if (!IsVisibleInTree()) return;
		
		// Close book
		if (@event.IsActionPressed(Common.Inputs.GameActions.Book.Back) || @event.IsActionPressed(Common.Inputs.GameActions.Book.CloseBook))
		{
			HideBook();
			return;
		}

		// Switch main tab right
		if (@event.IsActionPressed(Common.Inputs.GameActions.Book.TriggerRight))
		{
			if (Math.Abs(@event.GetActionStrength(Common.Inputs.GameActions.Book.TriggerRight) - 1.0) > double.Epsilon)
			{
				// Triggers will fire release and press events for each little bit of the trigger
				return;
			}

			var nextOrFirstTabIndex = (_tabContainer.CurrentTab + 1) % _tabContainer.GetChildCount();
			_tabContainer.CurrentTab = nextOrFirstTabIndex;
			return;
		}
		
		// Switch main tab left
		if (@event.IsActionPressed(Common.Inputs.GameActions.Book.TriggerLeft))
		{
			if (Math.Abs(@event.GetActionStrength(Common.Inputs.GameActions.Book.TriggerLeft) - 1.0) > double.Epsilon)
			{
				return;
			}

			var count = _tabContainer.GetChildCount();
			var prevOrLastTabIndex = (_tabContainer.CurrentTab - 1 + count) % count;
			_tabContainer.CurrentTab = prevOrLastTabIndex;
			return;
		}
	}

	private void UpdateInventory(Player.Player player, IInventory inventory)
	{
		_playerInventoryPage.UpdateInventories(player.Inventory.GetSubInventories());
	}

	private void ShowBook()
	{
		_logger.Info("Opening...");
		GameStateMachine.Instance.SetState(GameState.Book);
		UpdateInventory(Game.Player, Game.Player.Inventory);
		Show();
	}

	private void HideBook()
	{
		_logger.Info("Hiding...");
		GameStateMachine.Instance.SetState(GameState.FreeRoam);
		Hide();
	}
}
