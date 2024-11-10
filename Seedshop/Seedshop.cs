using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using untitledplantgame.Common;

public partial class Seedshop : CanvasLayer
{
	private readonly Logger _logger = new("Seedshop");
	private Label tooltip;
	private Dictionary<string, Dictionary<string, int>> seedData = new Dictionary<string, Dictionary<string, int>>()
	{
		{
			"Basil",
			new Dictionary<string, int>
			{
				{ "price", 10 },
				{ "available", 5 },
				{ "panel", 1 },
			}
		},
		{
			"Parsley",
			new Dictionary<string, int>
			{
				{ "price", 15 },
				{ "available", 3 },
				{ "panel", 2 },
			}
		},
		{
			"Mint",
			new Dictionary<string, int>
			{
				{ "price", 20 },
				{ "available", 2 },
				{ "panel", 3 },
			}
		},
		{
			"Cilantro",
			new Dictionary<string, int>
			{
				{ "price", 12 },
				{ "available", 8 },
				{ "panel", 4 },
			}
		},
		{
			"Oregano",
			new Dictionary<string, int>
			{
				{ "price", 14 },
				{ "available", 6 },
				{ "panel", 5 },
			}
		},
		{
			"Thyme",
			new Dictionary<string, int>
			{
				{ "price", 18 },
				{ "available", 4 },
				{ "panel", 6 },
			}
		},
		{
			"Rosemary",
			new Dictionary<string, int>
			{
				{ "price", 25 },
				{ "available", 2 },
				{ "panel", 7 },
			}
		},
		{
			"Sage",
			new Dictionary<string, int>
			{
				{ "price", 17 },
				{ "available", 7 },
				{ "panel", 8 },
			}
		},
		{
			"Chives",
			new Dictionary<string, int>
			{
				{ "price", 13 },
				{ "available", 10 },
				{ "panel", 9 },
			}
		},
		{
			"Dill",
			new Dictionary<string, int>
			{
				{ "price", 11 },
				{ "available", 9 },
				{ "panel", 10 },
			}
		},
		{
			"Lavender",
			new Dictionary<string, int>
			{
				{ "price", 30 },
				{ "available", 1 },
				{ "panel", 11 },
			}
		},
		{
			"Tarragon",
			new Dictionary<string, int>
			{
				{ "price", 22 },
				{ "available", 4 },
				{ "panel", 12 },
			}
		},
		{
			"Fennel",
			new Dictionary<string, int>
			{
				{ "price", 16 },
				{ "available", 6 },
				{ "panel", 13 },
			}
		},
		{
			"Marjoram",
			new Dictionary<string, int>
			{
				{ "price", 19 },
				{ "available", 5 },
				{ "panel", 14 },
			}
		},
		{
			"Lemon Balm",
			new Dictionary<string, int>
			{
				{ "price", 14 },
				{ "available", 8 },
				{ "panel", 15 },
			}
		},
		{
			"Chervil",
			new Dictionary<string, int>
			{
				{ "price", 12 },
				{ "available", 10 },
				{ "panel", 16 },
			}
		},
	};

	public override void _Ready()
	{
		this.Hide();
		EventBus.Instance.OnSeedshopOpened += OpenSeedshop;
		EventBus.Instance.OnSeedshopClosed += CloseSeedshop;
		var closeButton = GetNode<Button>("CloseButton");
		closeButton.Connect("pressed", new Callable(this, nameof(OnCloseButtonPressed)));

		var GridContainer = GetNode<GridContainer>("ColorRect/GridContainer");
		if (GridContainer != null)
		{
			foreach (Node ShopSlot in GridContainer.GetChildren())
			{
				var textureButton = ShopSlot.GetNode<TextureButton>("Panel/ColorRect/TextureButton");
				if (textureButton != null)
				{
					string name = ShopSlot.Name;
					textureButton.Connect("pressed", new Callable(this, name));
				}
			}
		}
		else
		{
			_logger.Error("GridContainer is null");
		}

		InitializeShopSlotLabels();
	}

	private void OpenSeedshop()
	{
		//work around for logging the event multiple times. On interacting, it gets called more than once
		if (!this.Visible)
		{
			this.Show();
			_logger.Debug("Seedshop opened.");
		}
	}

	private void CloseSeedshop()
	{
		if (this.Visible)
		{
			this.Hide();
			_logger.Debug("Seedshop closed.");
		}
	}

	private void CustomTooltip(string name)
	{
		tooltip = GetNode<Label>("Seedshop/Tooltip");
		tooltip.Text = name;
	}

	private void OnCloseButtonPressed()
	{
		CloseSeedshop();
	}

	private void InitializeShopSlotLabels()
	{
		foreach (KeyValuePair<string, Dictionary<string, int>> seed in seedData)
		{
			string panelName = "Panel" + seed.Value["panel"];
			// updateLabel(panelName, seed.Key, seed.Value["price"], seed.Value["available"]);
			Label nameLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/Name");
			Label priceLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/Price");
			Label availableLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/ColorRect/Available");
			nameLabel.Text = seed.Key;
			priceLabel.Text = seed.Value["price"].ToString();
			availableLabel.Text = seed.Value["available"].ToString();
		}
	}

	[Obsolete]
	private void updateLabel(string panelName, string name, int price, int available)
	{
		Label nameLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/Name");
		Label priceLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/Price");
		Label availableLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/ColorRect/Available");
		nameLabel.Text = name;
		priceLabel.Text = price.ToString();
		availableLabel.Text = available.ToString();
	}

	private void reduceAvailability(string name)
	{
		foreach (KeyValuePair<string, Dictionary<string, int>> seed in seedData)
		{
			if (seed.Key == name)
			{
				Label availableLabel = GetNode<Label>("ColorRect/GridContainer/Panel" + seed.Value["panel"] + "/Panel/ColorRect/Available");
				availableLabel.Text = seed.Value["available"].ToString();
			}
		}
	}

	private void SeedSold(int panel)
	{
		(string name, int price, int available) = GetSeedInfoByPanel(panel);
		//TODO: Implement currency system and compare to price
		if (available > 0)
		{
			seedData[name]["available"] -= 1;
			reduceAvailability(name);
			_logger.Debug("Bought a " + name + " seed.");
		}
		else
		{
			_logger.Debug("Not enough seeds available or not enough currency!");
		}
	}

	private (string name, int price, int available) GetSeedInfoByPanel(int panel)
	{
		foreach (var seed in seedData)
		{
			if (seed.Value["panel"] == panel)
			{
				return (seed.Key, seed.Value["price"], seed.Value["available"]);
			}
		}
		_logger.Error("Seed not found.");
		return ("", -1, -1);
	}

	private void Panel1()
	{
		SeedSold(1);
	}

	private void Panel2()
	{
		SeedSold(2);
	}

	private void Panel3()
	{
		SeedSold(3);
	}

	private void Panel4()
	{
		SeedSold(4);
	}

	private void Panel5()
	{
		SeedSold(5);
	}

	private void Panel6()
	{
		SeedSold(6);
	}

	private void Panel7()
	{
		SeedSold(7);
	}

	private void Panel8()
	{
		SeedSold(8);
	}

	private void Panel9()
	{
		SeedSold(9);
	}

	private void Panel10()
	{
		SeedSold(10);
	}

	private void Panel11()
	{
		SeedSold(11);
	}

	private void Panel12()
	{
		SeedSold(12);
	}

	private void Panel13()
	{
		SeedSold(13);
	}

	private void Panel14()
	{
		SeedSold(14);
	}

	private void Panel15()
	{
		SeedSold(15);
	}

	private void Panel16()
	{
		SeedSold(16);
	}
}
