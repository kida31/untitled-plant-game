using Godot;
using DialogueLine = untitledplantgame.Dialogue.Models.DialogueLine;

namespace untitledplantgame.Dialogue;

public partial class DialogueAnimation : Node
{
	private const float CharacterPerSecond = 40; // range 25 - 40

	[Export] private Timer _timer;
	public bool AnimationIsPlaying => _currentLetterIndex != -1;

	private int _currentLetterIndex;

	public DialogueAnimation()
	{
		AddChild(_timer = new Timer());
		_timer.OneShot = true;
	}

	/// <summary>
	/// Animates the dialogue line in a typewriter fashion.
	/// </summary>
	/// <param name="dialogueTextLabel">Label that shows the text</param>
	/// <param name="line">current line</param>
	public async void AnimateNextDialogueLine(RichTextLabel dialogueTextLabel, DialogueLine line)
	{
		_currentLetterIndex = 1;

		while (_currentLetterIndex != -1 && _currentLetterIndex <= line.dialogueText.Length)
		{
			dialogueTextLabel.VisibleCharacters = _currentLetterIndex;
			_currentLetterIndex++;
			_timer.Start(1 / CharacterPerSecond);
			await ToSignal(_timer, Timer.SignalName.Timeout);
		}

		_currentLetterIndex = -1;

		dialogueTextLabel.VisibleCharacters = -1;
	}

	/// <summary>
	/// Goes to the end of the current animation.
	/// </summary>
	public void StopAnimation()
	{
		_currentLetterIndex = -1;
	}
}
