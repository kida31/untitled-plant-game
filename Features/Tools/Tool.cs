using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Tools.ToolDatas;

namespace untitledplantgame.Tools;

// Problem, cast time logic needs to be in/by a Node
// Part of the logic (Use()) is in Tool. Logic should be in one place only.

// This has above average priority for reworking

/// <summary>
///		A base class for tools. Tools are items that can be used by the player.
///		Tool implementations implement the logic for the tool by overriding the virtual methods.
/// </summary>
public abstract partial class Tool : Resource, IDisplayData, IToolUseData
{
	public event Action FinishedCasting;
	[Export] public virtual string Name { get; protected set; }
	[Export(PropertyHint.MultilineText)] public virtual string Description { get; protected set; }
	[Export] public virtual Texture2D Icon { get; protected set; }
	[Export] public virtual float ChannelingTime { get; protected set; }
	[Export] public virtual float Radius { get; protected set; } = 12;
	[Export] public virtual float Range { get; protected set; } = 24;

	private ChannelingBar _channelingBar;
	private ToolHitScan _hitScanArea;

	private readonly Logger _logger = new("Tool");

	// Having every tool be channeling smells bad
	
	/// <summary>
	///		 Start channeling the tool.
	/// </summary>
	/// <param name="user"></param>
	public void StartChanneling(Player.Player user)
	{
		_logger.Debug("Start channeling");
		_OnStart(user);


		_hitScanArea = new ToolHitScan(Radius);
		_hitScanArea.TopLevel = true;
		_hitScanArea.Hit += (hits) => _OnInitialHit(user, hits);
		user.AddChild(_hitScanArea);
		_hitScanArea.GlobalPosition = user.GlobalPosition + user.FaceDirection * Range;
	}

	/// <summary>
	///		Cancel the channeling of the tool.
	/// </summary>
	/// <param name="user"></param>
	public void Cancel(Player.Player user)
	{
		_logger.Debug("Cancel channeling");
		if (IsInstanceValid(_channelingBar))
		{
			_channelingBar?.QueueFree();
		}

		_channelingBar = null;

		if (IsInstanceValid(_hitScanArea))
		{
			_hitScanArea?.QueueFree();
		}

		_hitScanArea = null;
	}

	/// <summary>
	///		This is called when the player starts casting the tool.
	/// </summary>
	/// <param name="user"></param>
	protected virtual void OnStart(Player.Player user)
	{
	}

	/// <summary>
	///		This is called when the player starts casting the tool. The hits are the nodes that the tool hit.
	///		This method should return true if the tool handled any hits.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="hits"></param>
	/// <returns></returns>
	protected virtual bool OnInitialHit(Player.Player user, Node2D[] hits)
	{
		return false;
	}

	/// <summary>
	///		 This is called when the player finishes casting the tool. The hits are the nodes that the tool hit.
	///		 This method should return true if the tool handled any hits.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="hits"></param>
	/// <returns></returns>
	protected virtual bool OnHit(Player.Player user, Node2D[] hits)
	{
		return false;
	}

	/// <summary>
	///		 This is called when the player finishes casting the tool and OnHit returned false or no hits were found.
	/// </summary>
	/// <param name="user"></param>
	protected virtual void OnMiss(Player.Player user)
	{
	}

	/// <summary>
	///		 This is always called at the end of the casting.
	/// </summary>
	/// <param name="user"></param>
	protected virtual void OnFinishCast(Player.Player user)
	{
	}

	private void _OnStart(Player.Player user)
	{
		_logger.Debug("Start casting");
		OnStart(user);
	}

	private void _OnInitialHit(Player.Player user, Node2D[] hits)
	{
		_logger.Debug("Initial hit");

		if (hits.Length > 0) OnInitialHit(user, hits);

		//Channeling bar as timer for the tool use
		_channelingBar = new ChannelingBar(user, ChannelingTime);
		_channelingBar.Visible = false;
		// Consider scanning again after StartChanneling
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
		OnFinishCast(user);
		FinishedCasting?.Invoke();
	}
}
