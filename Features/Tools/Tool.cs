using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Tools;

// Problem, cast time logic needs to be in/by a Node
// Part of the logic (Use()) is in Tool. Logic should be in one place only.
public abstract class Tool
{
	public event Action FinishedCasting;
	public float ChannelingTime => _channelingTime;

	private readonly float _radius;
	private readonly float _range;
	private readonly float _channelingTime;

	private Player.Player _user;
	private ChannelingBar _channelingBar;
	private ToolHitScan _hitScanArea;

	private readonly Logger _logger = new("Tool");

	protected Tool(float radius, float range, float channelingTime)
	{
		_radius = radius;
		_range = range;
		_channelingTime = channelingTime;
	}

	// Having every tool be channeling smells bad
	public void StartChanneling(Player.Player user)
	{
		_logger.Debug("Start channeling");
		_OnStart(user);


		_hitScanArea = new ToolHitScan(_radius);
		_hitScanArea.TopLevel = true;
		_hitScanArea.Hit += (hits) => _OnInitialHit(user, hits);
		user.AddChild(_hitScanArea);
		_hitScanArea.GlobalPosition = user.GlobalPosition + user.FaceDirection * _range;
	}

	public void Cancel(Player.Player user)
	{
		_logger.Debug("Cancel channeling");
		if (GodotObject.IsInstanceValid(_channelingBar))
		{
			_channelingBar?.QueueFree();
		}

		_channelingBar = null;

		if (GodotObject.IsInstanceValid(_hitScanArea))
		{
			_hitScanArea?.QueueFree();
		}

		_hitScanArea = null;
	}

	protected abstract void OnStart(Player.Player user);
	protected abstract bool OnInitialHit(Player.Player user, Node2D[] hits);
	protected abstract bool OnHit(Player.Player user, Node2D[] hits);
	protected abstract void OnMiss(Player.Player user);

	private void _OnStart(Player.Player user)
	{
		_logger.Debug("Start casting");
		OnStart(user);
	}

	private void _OnInitialHit(Player.Player user, Node2D[] hits)
	{
		_logger.Debug("Initial hit");

		if (hits.Length > 0) OnInitialHit(user, hits);

		_channelingBar = new ChannelingBar(user, _channelingTime);
		// TODO replace with actual new hit scan
		_channelingBar.Completed += () => _OnHit(user, hits);
		user.AddChild(_channelingBar);
	}

	private void _OnHit(Player.Player user, Node2D[] hits)
	{
		_logger.Debug("Actual hit");

		if (hits.Length == 0 || !OnHit(user, hits))
		{
			_OnMiss(user);
			return;
		}

		_OnFinishCast(user);
	}

	private void _OnMiss(Player.Player user)
	{
		_logger.Debug("Missed");
		OnMiss(user);
		_OnFinishCast(user);
	}

	private void _OnFinishCast(Player.Player user)
	{
		_logger.Debug("Finish casting");
		_channelingBar?.QueueFree();
		_channelingBar = null;
		FinishedCasting?.Invoke();
	}
}
