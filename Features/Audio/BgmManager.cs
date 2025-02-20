using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Audio;

/// <summary>
///     Manages the background music (BGM) for the game.
///     Cross-fades between tracks. There can only be one BgmManager in the scene.
///		Only one track can play at a time.
/// </summary>
public partial class BgmManager : Node
{
	public static BgmManager Instance { get; private set; }
	
	[Export] private float _crossFadeDuration;
	[Export] private AudioStream _defaultMusic;

	[ExportGroup("Node Setup")] [Export] private AudioStreamPlayer _audioStreamPlayerA;
	[Export] private AudioStreamPlayer _audioStreamPlayerB;

	private Tween _transitionTween;
	private AudioStream _currentBgm;
	private Logger _logger;

	public override void _Ready()
	{
		if (Instance != null)
		{
			_logger.Error("Multiple instances of BgmManager detected. Deleting this instance.");
			QueueFree();
			return;
		}
		
		Instance = this;
		_logger = new(this);
		Play(_defaultMusic); // Consider moving this to another class

		EventBus.Instance.BgmAreaChanged += OnBgmAreaChanged;
	}

	public override void _ExitTree()
	{
		EventBus.Instance.BgmAreaChanged -= OnBgmAreaChanged;
	}

	/// <summary>
	///     Plays the specified audio track.
	///     Cross-fades between tracks if there is already a track playing.
	///     Calling Play() with the same track will not restart the track.
	/// </summary>
	/// <param name="stream"></param>
	public void Play(AudioStream stream)
	{
		if (_currentBgm == stream)
		{
			return;
		}

		_logger.Info($"Playing \"{stream.ResourcePath?.GetFile().GetBaseName()}\"");
		_currentBgm = stream;
		CrossFade(stream);
	}

	private void OnBgmAreaChanged(IBgmArea area)
	{
		Play(area?.GetBgm());
	}

	[Obsolete("There is a new Godot Feature called AudioStreamInteractive. Use that instead.")]
	private void CrossFade(AudioStream nextTrack)
	{
		_logger.Debug($"Cross-fading from \"{_audioStreamPlayerA.Stream?.ResourcePath}\" to \"{nextTrack.ResourcePath}\"");
		if (_audioStreamPlayerA.Playing)
		{
			// From A to B track
			_audioStreamPlayerB.Stream = nextTrack;
			_audioStreamPlayerB.Play();
			FadeAToB().OnCompleted(_audioStreamPlayerA.Stop);
		}
		else
		{
			// From B to A track
			_audioStreamPlayerA.Stream = nextTrack;
			_audioStreamPlayerA.Play();
			FadeBToA().OnCompleted(_audioStreamPlayerB.Stop);
		}
	}

	private SignalAwaiter FadeAToB()
	{
		_transitionTween?.Stop();
		_transitionTween = CreateTween();
		_transitionTween.TweenMethod(Callable.From<float>(SetA), 1.0, 0.0, _crossFadeDuration);
		_transitionTween.Parallel().TweenMethod(Callable.From<float>(SetB), 0.0, 1.0, _crossFadeDuration);
		return ToSignal(_transitionTween, Tween.SignalName.Finished);
	}

	private SignalAwaiter FadeBToA()
	{
		_transitionTween?.Stop();
		_transitionTween = CreateTween();
		_transitionTween.TweenMethod(Callable.From<float>(SetB), 1.0, 0.0, _crossFadeDuration);
		_transitionTween.Parallel().TweenMethod(Callable.From<float>(SetA), 0.0, 1.0, _crossFadeDuration);
		return ToSignal(_transitionTween, Tween.SignalName.Finished);
	}

	private void SetA(float value)
	{
		_audioStreamPlayerA.VolumeDb = Mathf.LinearToDb(value);
	}

	private void SetB(float value)
	{
		_audioStreamPlayerB.VolumeDb = Mathf.LinearToDb(value);
	}
}
