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
	[ExportGroup("Node Setup")] [Export] private ToolCircle _wateringCanBlob;
	[Export] private ToolCircle _shearsBlob;
	[Export] private ToolCircle _shovelBlob;
	[Export] private ToolCircle _fishingRod;
	[Export] private ToolCircle _seedBagBlob;

	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);

		if (_wateringCanBlob == null) _logger.Error("WateringCanBlob is null.");
		if (_shearsBlob == null) _logger.Error("ShearsBlob is null.");
		if (_shovelBlob == null) _logger.Error("ShovelBlob is null.");
		if (_fishingRod == null) _logger.Error("FishingRod is null.");
		if (_seedBagBlob == null) _logger.Error("SeedBagBlob is null.");

		VisibilityChanged += OnVisibilityChanged;

		_logger.Info("Ready");
	}

	private void OnVisibilityChanged()
	{
		if (!IsVisibleInTree() || !IsNodeReady()) return;

		var tools = Game.Player?.Toolbelt?.Tools;

		if (_wateringCanBlob != null) _wateringCanBlob.Tool = tools?.FirstOrDefault(t => t is WateringCan);
		if (_shearsBlob != null) _shearsBlob.Tool = tools?.FirstOrDefault(t => t is Shears);
		if (_shovelBlob != null) _shovelBlob.Tool = tools?.FirstOrDefault(t => t is Shovel);
		if (_fishingRod != null) _fishingRod.Tool = tools?.FirstOrDefault(t => false /* is FishingRod */);
		if (_seedBagBlob != null) _seedBagBlob.Tool = tools?.FirstOrDefault(t => t is SeedBag);
	}
}
