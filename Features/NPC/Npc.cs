using Godot;

namespace untitledplantgame.NPC;

public abstract partial class Npc : AInteractable
{
	public abstract string GetNpcName();
	public abstract bool IsNpcInteractable();
	public abstract Texture2D GetNpcDefaultPortrait();
	public abstract void SetNpcDefaultPortrait(Texture2D newDefaultPortrait);
}
