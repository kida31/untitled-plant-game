using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Events;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC.NpcType;
using untitledplantgame.Shops;

namespace untitledplantgame.NPC;

public partial class OutOfStock : Node
{
	[Export] private DialogueResourceObject _outOfStock;
	[Export] private DialogueResourceObject _yesResponse; // Crossier's Note: Horrible. Sad. Why? I know why. It's sad.
	[Export] private StandardNpc _seedBoy;
	private bool _firstEncounter;
	
	public override void _Ready()
	{
		_seedBoy.AssignMethodToInteractionEvent(OpenAlternativeDialogue);
	}

	private async void OpenAlternativeDialogue()
	{
		await Task.Delay(1);
		
		if (_firstEncounter)
		{
			_firstEncounter = false;
		}
		else
		{
			SayOutOfStockIfOutOfStock();
		}
	}

	private void SayOutOfStockIfOutOfStock()
	{
		foreach (var line in _yesResponse._dialogueText)
		{
			if (line is not OpenSeedShop seedShop)
			{
				continue;
			}

			foreach (var item in seedShop.SeedShop.CurrentStock)
			{
				if (item != null)
				{
					return;
				}
					
				EventBus.Instance.InvokeStartingDialogue(_outOfStock);
			}
		}
	}
}
