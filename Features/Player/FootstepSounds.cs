using Godot;

namespace untitledplantgame.Player;

public partial class FootstepSounds : AudioStreamPlayer2D
{
	enum GroundType
	{
		Grass,
		Wood,
	}
	[Export]
	private float PlaybackCooldown = 0.2f;

	[Export] private AudioStream _grassSound;
	[Export] private AudioStream _woodSound;
	
	private Player _player;
	private float _currentPlaybackCooldown;
	
	public override void _Ready()
	{
		_player = GetParent<Player>();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsNodeReady()) return;
		
		_currentPlaybackCooldown -= (float)delta;
		if (_player.Velocity == Vector2.Zero)
		{
			Stop();
		}
		else
		{
			if (_currentPlaybackCooldown > 0f) return;
			_currentPlaybackCooldown = PlaybackCooldown;
			var ground = GetGroundType();
			switch (ground)
			{
				case GroundType.Grass:
					Stream = _grassSound;
					break;
				case GroundType.Wood:
					Stream = _woodSound;
					break;
			}
			Play();
		}
	}

	private GroundType GetGroundType()
	{
		return GroundType.Grass;
	}
}
