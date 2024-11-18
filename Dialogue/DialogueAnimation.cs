using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Dialogue;

public partial class DialogueAnimation : Node
{
	private const float CharacterPerSecond = 40; // range 25 - 40 
	public bool IsPlaying { get; private set; }

	[Export] private Timer _timer;
	private Logger _logger;
	public int CurrentLetterIndex;
	public bool AnimationIsPlaying => CurrentLetterIndex != -1;

	public DialogueAnimation()
	{
		_logger = new Logger(this);
		AddChild(_timer = new Timer());
		_timer.Timeout += PlayAnimation;
		_timer.OneShot = true;
	}

	private void PlayAnimation()
	{
		/*
		if (_currentLetterIndex < _text.Length)
		{
			GD.PrintRaw(_text[_currentLetterIndex]);
			_currentLetterIndex++;
		}
		*/
	}

	public async void AnimateNextDialogueLine(RichTextLabel dialogueTextLabel, DialogueLine line)
	{
		CurrentLetterIndex = 1;

		while (CurrentLetterIndex != -1 && CurrentLetterIndex <= line.dialogueText.Length)
		{
			dialogueTextLabel.VisibleCharacters = CurrentLetterIndex;
			CurrentLetterIndex++;
			_timer.Start(1/CharacterPerSecond);
			await ToSignal(_timer, Timer.SignalName.Timeout);
		}

		CurrentLetterIndex = -1;
		
		dialogueTextLabel.VisibleCharacters = -1;
	}
}
