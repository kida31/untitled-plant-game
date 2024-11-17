using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Dialogue;

public partial class DialogueAnimation : Node
{
	public bool IsPlaying { get; private set; }

	[Export] private Timer _timer;
	private Logger _logger;
	private char[] _text;
	private int _currentLetterIndex;

	public DialogueAnimation()
	{
		_logger = new Logger(this);
		AddChild(_timer = new Timer());
		_timer.Timeout += PlayAnimation;
	}

	private void PlayAnimation()
	{
		GD.Print("timeout");
		if (_currentLetterIndex < _text.Length)
		{
			GD.PrintRaw(_text[_currentLetterIndex]);
			_currentLetterIndex++;
		}
	}

	void Reset()
	{
		StopAnimation();
		_currentLetterIndex = 0;
	}

	public void SetLine(string line)
	{
		_text = line.ToCharArray();
		Reset();
		_timer.Start();
	}

	public void SkipAnimation()
	{
		_logger.Info("Skipping animation.");
		//displays the dialogue all at once
		if (_text == null)
		{
			_logger.Error("Text is null.");
			return;
		}

		GD.PrintRaw(_text.ToString().Substring(_currentLetterIndex, _text.Length));
		Reset();
	}

	void StopAnimation()
	{
		_timer.Stop();
		IsPlaying = false;
	}
}
