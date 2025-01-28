using Godot;
using untitledplantgame.NPC;

namespace untitledplantgame.Fishing;

public partial class FishingSpot : AInteractable
{
	// This is a node. Arbitrary
	[Export] private FishingWaitingGame _fishingWaitingGame;
	[Export] private untitledplantgame.Fishing.FishingGame _fishingGame;
	
	public override string ActionName => "Throw rod";

	public override void Interact()
	{
		
	}
}
