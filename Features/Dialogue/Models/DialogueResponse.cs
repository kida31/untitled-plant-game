using Godot;

namespace untitledplantgame.Dialogue.Models;

[GlobalClass]
public partial class DialogueResponse : Resource //DO NOT rename variables or the resource will break
{
	[Export] public string _responseButton { get; set; } 

	[Export] public DialogueResourceObject _responseDialogue { get; set; }
}
