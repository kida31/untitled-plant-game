using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using untitledplantgame.Common;

public partial class OLDSeedshop : CanvasLayer
{
	private int playerCurrency = 100;

	private readonly Logger _logger = new("OLDSeedshop");

	private Dictionary<string, Dictionary<string, int>> seedData = new Dictionary<string, Dictionary<string, int>>()
	{
		{
			"Carrot",
			new Dictionary<string, int>
			{
				{ "price", 10 },
				{ "available", 5 },
				{ "owned", 0 },
			}
		},
		{
			"Tomato",
			new Dictionary<string, int>
			{
				{ "price", 15 },
				{ "available", 3 },
				{ "owned", 0 },
			}
		},
		{
			"Pumpkin",
			new Dictionary<string, int>
			{
				{ "price", 20 },
				{ "available", 2 },
				{ "owned", 0 },
			}
		},
	};

	public override void _Ready()
	{
		this.Hide();
		EventBus.Instance.OnSeedshopOpened += OpenSeedshop;
		var CloseButton = GetNode<Button>("ColorRect/CloseButton");
		CloseButton.Connect("pressed", new Callable(this, nameof(OnCloseButtonPressed)));

		var CarrotButton = GetNode<TextureButton>("ColorRect/GridContainer/Carrots/TextureButton");
		CarrotButton.Connect("pressed", new Callable(this, nameof(OnCarrotButtonPressed)));

		var TomatoButton = GetNode<TextureButton>("ColorRect/GridContainer/Tomatoes/TextureButton");
		TomatoButton.Connect("pressed", new Callable(this, nameof(OnTomatoButtonPressed)));

		var PumpkinButton = GetNode<TextureButton>("ColorRect/GridContainer/Pumpkins/TextureButton");
		PumpkinButton.Connect("pressed", new Callable(this, nameof(OnPumpkinButtonPressed)));

		UpdateSeedLabels("Carrot");
		UpdateSeedLabels("Tomato");
		UpdateSeedLabels("Pumpkin");
		UpdateCurrencyLabel();
	}

	private void OpenSeedshop()
	{
		this.Show();
		_logger.Debug("Seedshop opened.");
	}

	private void OnCloseButtonPressed()
	{
		this.Hide();
	}

	private void OnSeedButtonPressed(string seedType)
	{
		_logger.Debug("Buying a " + seedType + " seed.");
		var seedInfo = seedData[seedType];
		if (seedInfo["available"] > 0 && playerCurrency >= seedInfo["price"])
		{
			playerCurrency -= seedInfo["price"];
			seedInfo["available"] -= 1;
			seedInfo["owned"] += 1;

			UpdateSeedLabels(seedType);
			UpdateCurrencyLabel();
			_logger.Debug("Bought a " + seedType + " seed. Total Owned: " + seedInfo["owned"]);
		}
		else
		{
			_logger.Debug("Not enough seeds available or not enough currency!");
		}
	}

	private void OnCarrotButtonPressed()
	{
		OnSeedButtonPressed("Carrot");
	}

	private void OnTomatoButtonPressed()
	{
		OnSeedButtonPressed("Tomato");
	}

	private void OnPumpkinButtonPressed()
	{
		OnSeedButtonPressed("Pumpkin");
	}

	private void UpdateCurrencyLabel()
	{
		var moneyLabel = GetNode<Label>("ColorRect/money");
		moneyLabel.Text = "Money: " + playerCurrency;
	}

	private void UpdateSeedLabels(string seedType)
	{
		Label availableLabel = null;
		Label ownedLabel = null;
		Label priceLabel = null;
		switch (seedType)
		{
			case "Carrot":
				availableLabel = GetNode<Label>("ColorRect/GridContainer/Carrots/available");
				ownedLabel = GetNode<Label>("ColorRect/GridContainer/Carrots/owned");
				priceLabel = GetNode<Label>("ColorRect/GridContainer/Carrots/price");
				break;

			case "Tomato":
				availableLabel = GetNode<Label>("ColorRect/GridContainer/Tomatoes/available");
				ownedLabel = GetNode<Label>("ColorRect/GridContainer/Tomatoes/owned");
				priceLabel = GetNode<Label>("ColorRect/GridContainer/Tomatoes/price");
				break;

			case "Pumpkin":
				availableLabel = GetNode<Label>("ColorRect/GridContainer/Pumpkins/available");
				ownedLabel = GetNode<Label>("ColorRect/GridContainer/Pumpkins/owned");
				priceLabel = GetNode<Label>("ColorRect/GridContainer/Pumpkins/price");
				break;
		}

		if (availableLabel != null && ownedLabel != null && priceLabel != null)
		{
			availableLabel.Text = "Available: " + seedData[seedType]["available"];
			ownedLabel.Text = "Owned: " + seedData[seedType]["owned"];
			priceLabel.Text = "Price: " + seedData[seedType]["price"];
		}
	}
}
