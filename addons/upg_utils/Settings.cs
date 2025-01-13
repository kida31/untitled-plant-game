using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.addons.upg_utils;

public static class Settings
{
	public static class Logging
	{
		private const string LogLevelKey = Logger.LogLevelSetting;
	
		/// <summary>
		/// Returns the current LogLevel defined in ProjectSettings. Returns null if nothing is defined.
		/// 
		/// This is a wrapper for ProjectSettings.GetSetting(...) that casts the value to the enum value.
		/// </summary>
		/// <returns></returns>
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
	
		/// <summary>
		/// Sets the LogLevel in ProjectSettings.
		///
		/// This is a wrapper for ProjectSettings.SetSetting(...) that sets the value as a string.
		/// </summary>
		/// <param name="logLevel"></param>
		public static void SetLogLevel(LogLevel logLevel)
		{
			ProjectSettings.SetSetting(LogLevelKey, logLevel.ToString());
			ProjectSettings.Save();
			GD.Print($"Set {LogLevelKey} to \"{logLevel.ToString()}\"");
		}
	}
}
