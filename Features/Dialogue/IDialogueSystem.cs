using System;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue;

public interface IDialogueSystem
{
	/// <summary>
	/// Event that is triggered when the dialogue starts;
	/// </summary>
	event Action<DialogueResourceObject> OnDialogueBlockStarted;

	/// <summary>
	/// Event that is triggered when the dialogue ends.
	/// </summary>
	event Action<DialogueResourceObject> OnDialogueEnd;
	
	/// <summary>
	/// Event that is triggered when the player can respond.
	/// </summary>
	event Action<string[]> OnResponding;

	/// <summary>
	/// Start the dialogue.
	/// </summary>
	/// <param name="dialogue"></param>
	//void StartDialog(string dialogue);

	void StartDialog(DialogueResourceObject dialogue);
	
	/// <summary>
	/// Invoke Response event to display responses.
	/// Set state to Responding.
	/// If there are no responses, end the dialogue.
	/// </summary>
	void GetResponses();
	
	/// <summary>
	/// Called whenever player chooses a response.
	/// </summary>
	void InsertSelectedResponse(string response);
}
