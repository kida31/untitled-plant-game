using System;

namespace untitledplantgame.Dialogue;

public interface IDialogueSystem
{
	/// <summary>
	/// Event that is triggered when the dialogue starts;
	/// </summary>
	event Action<DialogueResourceObject> OnDialogueStart;

	/// <summary>
	/// Event that is triggered when the dialogue ends.
	/// </summary>
	event Action<DialogueResourceObject> OnDialogueEnd;

	/// <summary>
	/// Start the dialogue.
	/// </summary>
	/// <param name="dialogue"></param>
	void StartDialog(string dialogue);
}
