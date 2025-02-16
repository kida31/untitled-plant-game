using Godot;

namespace untitledplantgame.Audio;

/// <summary>
///		Interface for areas that have a specific background music (BGM).
/// </summary>
public interface IBgmArea
{
	AudioStream GetBgm();
}
