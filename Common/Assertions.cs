using System;

namespace untitledplantgame.Common;

[Singleton]
public class Assertions
{
	private class AssertionError: Exception
	{
		public AssertionError(string message) : base(message) { }
	}
	
	private static Assertions _instance;
	private static Assertions Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new Assertions();
			}

			return _instance;
		}
	}
	
	private Logger _logger = new Logger("Assertions");

	private Assertions() { }
	
	public static void AssertTrue(bool condition, string message="")
	{
		if (!condition)
		{
			Instance._logger.Debug(message);
			// throw new AssertionError(message);
		}
	}
}
