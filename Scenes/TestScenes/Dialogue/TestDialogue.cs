using Godot;
using untitledplantgame.Common;
using DialogueResourceObject = untitledplantgame.Dialogue.Models.DialogueResourceObject;

namespace untitledplantgame.Scenes.TestScenes.Dialogue;

public partial class TestDialogue : Node2D
{
	[Export] private DialogueResourceObject _exampleDialogue;
	[Export] private DialogueResourceObject[] _followupDialogue;
	[Export] private Button _button;
	
	private readonly Logger _logger = new("TutorialDialogue");
	private int i = -1;

	public override void _Ready()
	{
		EventBus.Instance.InvokeStartingDialogue(_exampleDialogue._dialogueId);
		_button.Pressed += LoadNextDialogue;
	}

	private void LoadNextDialogue()
	{
		if(i < _followupDialogue.Length)
		{
			i++;
			EventBus.Instance.InvokeStartingDialogue(_followupDialogue[i]._dialogueId);
			_button.FocusMode = Control.FocusModeEnum.None;
			_logger.Debug($"current index {i}");
		}
		else
		{
			_logger.Debug("no more followup Dialogues");
		}
	}
}
