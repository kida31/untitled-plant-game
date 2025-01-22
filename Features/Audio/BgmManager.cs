using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame;

public partial class BgmManager : Node
{
	private const float MuteDb = -80;
	private const float DefaultDb = 0;
	private const string VolumeDbPath = "volume_db";

	[Export] private float _crossfadeDuration;
	[Export] AudioStreamPlayer _audioStreamPlayerA;
	[Export] AudioStreamPlayer _audioStreamPlayerB;

	[Export] AudioStream _defaultMusic;

	private Tween _tweenA;
	private Tween _tweenB;

	private IBgmArea _currentBgmArea;

	private Logger _logger;

	public override void _Ready()
	{
		_logger = new(this);
		EventBus.Instance.BgmAreaChanged += OnBgmAreaChanged;

		CrossFade(_defaultMusic);
	}

	private void OnBgmAreaChanged(IBgmArea area)
	{
		if (_currentBgmArea == area)
		{
			return;
		}
		_currentBgmArea = area;
		CrossFade(area.GetBgm());
	}

	private void CrossFade(AudioStream nextTrack)
	{
		_logger.Info($"Crossfading from {_audioStreamPlayerA.Stream?.ResourcePath} to {nextTrack.ResourcePath}");
		if (_audioStreamPlayerA.Playing)
		{
			// From A to B track
			// _logger.Info("Play B");
			_audioStreamPlayerB.Stream = nextTrack;
			_audioStreamPlayerB.Play();
			FadeAToB().OnCompleted(() =>
			{
				// _logger.Info("Stop A");
				_audioStreamPlayerA.Stop();
			});
		}
		else
		{
			// From B to A track
			// _logger.Info("Play A");
			_audioStreamPlayerA.Stream = nextTrack;
			_audioStreamPlayerA.Play();
			FadeBToA().OnCompleted(() =>
			{
				// _logger.Info("Stop B");
				_audioStreamPlayerB.Stop();
			});
		}
	}

	private SignalAwaiter FadeAToB()
	{
		_tweenA?.Stop();
		_tweenA = CreateTween();
		_tweenA.TweenMethod(Callable.From<float>(SetA), 1.0, 0.0, _crossfadeDuration);
		// _logger.Info("Fade out A");

		_tweenB?.Stop();
		_tweenB = CreateTween();
		_tweenB.TweenMethod(Callable.From<float>(SetB), 0.0, 1.0, _crossfadeDuration);
		// _logger.Info("Fade in B");

		return ToSignal(_tweenA, Tween.SignalName.Finished);
	}

	private SignalAwaiter FadeBToA()
	{
		_tweenB?.Stop();
		_tweenB = CreateTween();
		_tweenB.TweenMethod(Callable.From<float>(SetB), 1.0, 0.0, _crossfadeDuration);
		// _logger.Info("Fade out B");

		_tweenA?.Stop();
		_tweenA = CreateTween();
		_tweenA.TweenMethod(Callable.From<float>(SetA), 0.0, 1.0, _crossfadeDuration);
		// _logger.Info("Fade in A");

		return ToSignal(_tweenB, Tween.SignalName.Finished);
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
