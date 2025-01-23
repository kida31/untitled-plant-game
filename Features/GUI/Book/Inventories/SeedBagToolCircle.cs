using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Tools;

namespace untitledplantgame.GUI.Book.Inventories;

public partial class SeedBagToolCircle : ToolCircle
{
	[Export] private TextureRect _seedImage;

	private Action _onInventoryChangedHandler;
	
	public override Tool Tool
	{
		get => base.Tool;
		set
		{
			base.Tool = value;
			if (Tool != null)
			{
				_seedImage.Show();
			}
			else
			{
				_seedImage.Hide();
			}
		}
	}

	public override string Title
	{
		get
		{
			var seed = GetFirstSeed();
			if (seed != null)
			{
				return $"{base.Title} ({seed.Name})";
			}
			return base.Title;
		}
	}

	public override void _Ready()
	{
		VisibilityChanged += OnVisibilityChanged;
	}

	private void OnVisibilityChanged()
	{
		if (!IsVisibleInTree() || !IsNodeReady()) return;
		
		// Unsubscribe from the old event, if any
		Game.Player.Inventory.InventoryChanged -= _onInventoryChangedHandler;
		Game.Player.Inventory.InventoryChanged += _onInventoryChangedHandler;
		OnInventoryChanged();
	}

	private void OnInventoryChanged()
	{

		_seedImage.Texture = GetFirstSeed()?.Icon;
	}

	private static IItemStack GetFirstSeed()
	{
		var seedInventory = Game.Player.Inventory.GetInventory(ItemCategory.Seed);
		return seedInventory.FirstOrDefault(it => it != null);
	}
}
