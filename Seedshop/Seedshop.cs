using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;

public partial class Seedshop : CanvasLayer
{
	[Export]
	private Button closeButton;
	private readonly Logger _logger = new("Seedshop");
	private Label tooltip;
	//TODO remove Dictionary from Seedshop and use a resource file or something else instead
	//TODO implement parser for resource file
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
		closeButton.Pressed += CloseSeedshop;

		var GridContainer = GetNode<GridContainer>("ColorRect/GridContainer");
		if (GridContainer != null)
		{
			var shopSlots = GridContainer.GetChildren();
			for (int i = 0; i < shopSlots.Count; i++)
			{
				var shopSlot = shopSlots[i];
				var textureButton = shopSlot.GetNode<TextureButton>("Panel/ColorRect/TextureButton");
				if (textureButton != null)
				{
					var number = i + 1;
					textureButton.Pressed += () => SeedSold(number);
				}
			}
		}
		else
		{
			_logger.Error("GridContainer is null");
		}

		foreach (KeyValuePair<string, Dictionary<string, int>> seed in seedData)
		{
			string panelName = "Panel" + seed.Value["panel"];
			var nameLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/Name");
			var priceLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/Price");
			var availableLabel = GetNode<Label>("ColorRect/GridContainer/" + panelName + "/Panel/ColorRect/Available");
			nameLabel.Text = seed.Key;
			priceLabel.Text = seed.Value["price"].ToString();
			availableLabel.Text = seed.Value["available"].ToString();
		}
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
}
