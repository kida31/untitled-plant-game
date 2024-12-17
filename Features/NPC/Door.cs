using System;
using System.Collections;
using Godot;

public partial class Door : AInteractable
{
	[Export]
	public string pathToNewScene { get; private set; }

	[Export]
	public string entryDoorName { get; private set; }

	public override void Interact()
	{
		SceneManager.Instance.SwapScenes(pathToNewScene, this);
	}
}
