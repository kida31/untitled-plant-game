using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.HUDs;
using untitledplantgame.Tools;

namespace untitledplantgame.GUI.Book.Inventories;

/// <summary>
///		ToolsSection is a HBoxContainer that displays the tools in the player's inventory.
///		Each tool type has a dedicated ToolBlobView.
/// </summary>
public partial class ToolsSection : HBoxContainer
{
	[ExportGroup("Node Setup")]
	[Export] private ToolBlobView _wateringCanBlob;
	[Export] private ToolCircle _shearsBlob;
	[Export] private ToolCircle _shovelBlob;
	[Export] private ToolCircle _fishingRod;
	[Export] private ToolCircle _seedBagBlob;
	
	private Logger _logger;

	public override void _Ready()
	{
		VisibilityChanged += OnVisibilityChanged;
		_logger = new Logger(this);
		_logger.Info("Ready");
		if (_wateringCanBlob == null) _logger.Warn("WateringCanBlob is null.");
		if (_shearsBlob == null) _logger.Warn("ShearsBlob is null.");
		if (_shovelBlob == null) _logger.Warn("ShovelBlob is null.");
		if (_fishingRod == null) _logger.Warn("FishingRod is null.");
		if (_seedBagBlob == null) _logger.Warn("SeedBagBlob is null.");
	}

	private void OnVisibilityChanged()
	{
		if (!IsVisibleInTree() || !IsNodeReady()) return;

		var tools = Game.Player?.Toolbelt?.Tools;
		if (_wateringCanBlob == null || _shearsBlob == null || _shovelBlob == null || _fishingRod == null || _seedBagBlob == null)
		{
			
			GD.PushError("AWGAKLWGJLK");
			_logger.Error("One or more ToolBlobViews are null.");
		}
		// _wateringCanBlob.Tool = tools?.FirstOrDefault(t => t is WateringCan);
		_shearsBlob.Tool = tools?.FirstOrDefault(t => t is Shears);
		_shovelBlob.Tool = tools?.FirstOrDefault(t => t is Shovel);
		_fishingRod.Tool = tools?.FirstOrDefault(t => false /* is FishingRod */);
		_seedBagBlob.Tool = tools?.FirstOrDefault(t => t is SeedBag);
	}
}
