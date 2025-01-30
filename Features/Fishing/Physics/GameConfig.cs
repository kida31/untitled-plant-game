using Godot;

namespace untitledplantgame.Fishing.Physics;


[GlobalClass]
public partial class GameConfig : Resource
{
	/// <summary>
	///		Speed of the fish in units per second
	/// </summary>
	[Export] public float FishSpeed { get; private set; }
	
	/// <summary>
	///		 Speed of the fish (in opposite direction) in units per second when it's hooked
	/// </summary>
	[Export] public float PullSpeed { get; private set; }
	
	/// <summary>
	///		 Width of the hook in units
	/// </summary>
	[Export] public float HookWidth { get; private set; }
	
	/// <summary>
	///		Speed of the hook in units per second when moving freely
	/// </summary>
	[Export] public float HookSpeed { get; private set; }
	
	/// <summary>
	///		 Speed of the hook in units per second when pulling the fish
	/// </summary>
	[Export] public float HookSpeedAgainstFish { get; private set; }
	
	/// <summary>
	///		Progress decay per second when not pulling the fish
	/// </summary>
	[Export] public float ProgressDecayPerSecond { get; private set; }
	
	/// <summary>
	///		Progress gain per second when pulling the fish
	/// </summary>
	[Export] public float ProgressPullingPerSecond { get; private set; }
}
