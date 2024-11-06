using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.ResourceData;

[Singleton]
public partial class ResourceManager : Node
{
	public static ResourceManager Instance { get; private set; }

	private readonly Logger _logger = new("ResourceManager");

	public override void _Ready()
	{
		Instance ??= this;
	}

	public T[] LoadFromDirectory<T>(string dirPath)
		where T : Resource
	{
		var fileNames = DirAccess.GetFilesAt(dirPath);

		return fileNames
			.Select(fileName => dirPath + "/" + fileName)
			.Select(filePath =>
			{
				_logger.Info(filePath);
				return GD.Load(filePath) as T;
			})
			.ToArray();
	}

	public string[] LoadDirectoriesFromDirectory(string dirPath, List<string> results)
	{
		var dirNames = DirAccess.GetDirectoriesAt(dirPath);
		results.AddRange(dirNames.Select(d => dirPath + "/" + d));

		if (dirNames.Length <= 0)
			return results.ToArray();

		foreach (var dirName in dirNames)
		{
			LoadDirectoriesFromDirectory(dirPath + "/" + dirName, results);
		}

		return results.ToArray();
	}
}
