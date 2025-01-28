using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.NPC.NpcType;

public partial class SimpleTalkingObject : AInteractable
{
	[Export] private CompressedTexture2D _portraitTexture;
	[Export] private string _name;

	/// <summary>
	///		Newline separated text that the NPC will say when interacted with.
	/// </summary>
	[Export(PropertyHint.MultilineText)] private string _dialog;

	public override string ActionName => "Inspect";

	private DialogueResourceObject _dialogObject;

	public override void _Ready()
	{
		base._Ready();

		// Create dialog object from the exported variables
		var lines = _dialog
			.Split("\n", StringSplitOptions.RemoveEmptyEntries)
			.Select(line =>
			{
				var dialogLine = new DialogueLine();
				dialogLine.dialogueText = line;
				dialogLine.DialogueExpression = _portraitTexture;
				dialogLine.speakerName = _name;
				return dialogLine;
			});
		_dialogObject = new DialogueResourceObject();
		_dialogObject._responses = Array.Empty<DialogueResponse>();
		_dialogObject._dialogueText = lines.ToArray();
	}

	public override void Interact()
	{
		EventBus.Instance.InvokeStartingDialogue(_dialogObject);
	}
}
