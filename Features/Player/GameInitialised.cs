using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Quests;

namespace untitledplantgame.Player;

public partial class GameInitialised : Node
{
	[Export] private DialogueResourceObject _introMonologue;
	[Export] private QuestLine _introQuestLine;
	[Export] private bool _skipDialogue;
	
	private const int DelayAmount = 100;
	
	public override void _Ready()
	{
		QuestController.Instance.StartQuestLine(_introQuestLine);
		if (_skipDialogue)
		{
			return;
		}
		//WaitForSmoothness();
		EventBus.Instance.InvokeStartingDialogue(_introMonologue);
	}
	
	private async void WaitForSmoothness()
	{
		await Task.Delay(DelayAmount);
		EventBus.Instance.InvokeStartingDialogue(_introMonologue);
	}
}
