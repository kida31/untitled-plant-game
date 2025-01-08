using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.addons.upg_utils;

public class Settings
{
	public class Logging
	{
		private const string LogLevelKey = Logger.LogLevelSetting;
	
		public static LogLevel? GetLogLevel()
		{
			if (!ProjectSettings.HasSetting(LogLevelKey))
			{
				return null;
			}

			var logLevelName = ProjectSettings.GetSetting(LogLevelKey).AsString();
			if (Enum.TryParse(logLevelName, out LogLevel logLevel))
			{
				return logLevel;
			}
		
			return null;
		}
	
		public static void SetLogLevel(LogLevel logLevel)
		{
			ProjectSettings.SetSetting(LogLevelKey, logLevel.ToString());
			GD.Print("Set log level to " + logLevel.ToString() + " in project settings.");
		}
	}
}
