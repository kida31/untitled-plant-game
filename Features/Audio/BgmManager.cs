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

	private Tween _tweenA;
	private Tween _tweenB;
	
	private IBgmArea _currentBgmArea;

	private Logger _logger;
	
	public override void _Ready()
	{
		_logger = new(this);
		EventBus.Instance.BgmAreaChanged += OnBgmAreaChanged;
		
		AudioStreamPlayer.SignalName.
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
		_tweenA?.Stop();
		_tweenA = CreateTween();
		_tweenA.TweenProperty(_audioStreamPlayerB, VolumeDbPath, MuteDb, _crossfadeDuration);
		
		_tweenB?.Stop();
		_tweenB = CreateTween();
		_tweenB.TweenProperty(_audioStreamPlayerA, VolumeDbPath, DefaultDb, _crossfadeDuration);
		
		return ToSignal(_tweenA, Tween.SignalName.Finished);
	}

	private SignalAwaiter FadeBToA()
	{
		_tweenB?.Stop();
		_tweenB = CreateTween();
		_tweenB.TweenProperty(_audioStreamPlayerB, VolumeDbPath, MuteDb, _crossfadeDuration);
		
		_tweenA?.Stop();
		_tweenA = CreateTween();
		_tweenA.TweenProperty(_audioStreamPlayerA, VolumeDbPath, DefaultDb, _crossfadeDuration);
		
		return ToSignal(_tweenB, Tween.SignalName.Finished);
	}
}
