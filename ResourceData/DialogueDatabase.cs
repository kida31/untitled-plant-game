using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;

namespace untitledplantgame.ResourceData;

public partial class DialogueDatabase : Node, IDatabase<DialogueResourceObject>
{
	private const string _dialogueResourcePath = "res://ResourceData/Resources/Dialogue";
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
		var dialogues = GetAllResources();
		return dialogues.FirstOrDefault(dialogue => dialogue._dialogueId == name);
	}

	public DialogueResourceObject GetResourceById(int id)
	{
		throw new NotImplementedException();
	}

	public DialogueResourceObject[] GetAllResources()
	{
		var subDirectories = DirAccess.GetDirectoriesAt(DirPath);
		return subDirectories.Select(GetResourceByName).ToArray();
	}
}
