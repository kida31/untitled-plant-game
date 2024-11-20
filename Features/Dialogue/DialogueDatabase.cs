using System;
using System.IO;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;

namespace untitledplantgame.ResourceData;

public partial class DialogueDatabase : Node, IDatabase<DialogueResourceObject>
{
	private const string DialogueResourcePath = "res://ResourceData/Resources/Dialogue/";
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

	public string DirPath => DialogueResourcePath;

	/// <summary>
	/// Get a dialogue resource by its string id.
	/// </summary>
	/// <param name="name">Dialogue id</param>
	/// <returns></returns>
	public DialogueResourceObject GetResourceByName(string name)
	{
		var dialogues = GetAllResources();
		return dialogues.FirstOrDefault(dialogue => dialogue._dialogueId == name);
	}

	public DialogueResourceObject GetResourceById(int id)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Get all dialogue resources.
	/// </summary>
	/// <returns></returns>
	public DialogueResourceObject[] GetAllResources()
	{
		var subDirectories = DirAccess.GetFilesAt(DirPath);
		return subDirectories.Select(s => GD.Load<DialogueResourceObject>(Path.Join(DirPath, s))).ToArray();
	}
}
