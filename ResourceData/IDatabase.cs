using Godot;

namespace untitledplantgame.ResourceData;

public interface IDatabase<T> where T: Resource
{
	// Path to the directory containing the specific resources
	// e.g. "res://Resources/Items" for ItemDatabase or "res://Resources/Dialoue" for DialogueDatabase
	string DirPath { get; set; } //TODO do we need this?
	
	/// <summary>
	/// Get resource of type T by name. If no resource is found, return null.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	T GetResourceByName(string name);
	
	/// <summary>
	/// Get resource of type T by id. If no resource is found, return null.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	T GetResourceById(int id);
	
	/// <summary>
	/// Get all resources of type T. If no resources are found, return an empty array.
	/// </summary>
	/// <returns></returns>
	T[] GetAllResources();
}
