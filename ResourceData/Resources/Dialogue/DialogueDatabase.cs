using Godot;
using System;
using System.Collections.Generic;
using untitledplantgame.Dialogue;
using untitledplantgame.ResourceData;

public partial class DialogueDatabase : Node
{
	public static DialogueDatabase Instance { get; private set; }
	
	String _dialogueResourcePath = "res://ResourceData/Resources/Dialogue";
	List<DialogueResourceObject> _dialogueResources = new List<DialogueResourceObject>();
	
	ResourceManager _resourceManager = ResourceManager.Instance;

	public override void _Ready()
	{
		var directories = _resourceManager.LoadDirectoriesFromDirectory(_dialogueResourcePath, new List<string>());
		foreach (var directory in directories)
		{
			_dialogueResources.AddRange(_resourceManager.LoadFromDirectory<DialogueResourceObject>(directory));
		}
	}

	public DialogueResourceObject LoadDialogueResource(int id)
	{
		// Load dialogue from file
		return null;
	}
}
