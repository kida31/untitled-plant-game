using System.Collections.Generic;
using Godot;

namespace untitledplantgame.ResourceData;

public interface IDatabase<T> where T: Resource
{
	// Path to the directory containing the specific resources
	// e.g. "res://Resources/Items" for ItemDatabase or "res://Resources/Dialoue" for DialogueDatabase
	string DirPath { get; set; } //TODO do we need this?
	
	//Get resource of type T by name
	T GetResourceByName(string name);
	
	//Get resource of type T by id
	T GetResourceById(int id);
	
	//Get all resources of type T
	T[] GetAllResources();
}
