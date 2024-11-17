using Godot;
using System;
using System.Collections.Generic;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;
using untitledplantgame.ResourceData;

public partial class DialogueDatabase : Node, IDatabase<DialogueResourceObject>
{
	const string _dialogueResourcePath = "res://ResourceData/Resources/Dialogue";
	public static DialogueDatabase Instance { get; private set; }

	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			_logger.Error("There are multiple instances of DialogueDatabase");
			QueueFree();
		}
	}

	public string DirPath => _dialogueResourcePath;

	public DialogueResourceObject GetResourceByName(string name)
	{
		throw new NotImplementedException();
	}

	public DialogueResourceObject GetResourceById(int id)
	{
		throw new NotImplementedException();
	}

	public DialogueResourceObject[] GetAllResources()
	{
		throw new NotImplementedException();
	}
}
