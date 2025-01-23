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

	public override void _Ready()
	{
		Game.Player.Inventory.InventoryChanged += OnInventoryChanged;
	}

	private void OnInventoryChanged()
	{
		var seedInventory = Game.Player.Inventory.GetInventory(ItemCategory.Seed);
		var seed = seedInventory.FirstOrDefault(it => it != null);
		_seedImage.Texture = seed?.Icon;
	}
}
