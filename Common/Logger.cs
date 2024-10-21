﻿using Godot;
using System;

namespace untitledplantgame.Common;

enum LogLevel
{
    Info,
    Debug,
    Warn,
    Error
}

public class Logger
{
    private readonly string _logFilePath;
    private readonly string _name;

    public Logger(string name)
    {
        _name = name;

        // Check with Regex if name contains only letters and numbers
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z0-9]+$"))
        {
            throw new ArgumentException("Name can only contain letters and numbers");
        }

        // Write logs to file when it's not a debug build
        if (!OS.IsDebugBuild())
        {
            // Create directory if it doesn't exist
            var dirPath =
                System.IO.Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "logs");
            System.IO.Directory.CreateDirectory(dirPath);

            _logFilePath = System.IO.Path.Combine(dirPath, DateTime.Now.ToString("yyyy-MM-dd"));
        }
        else
        {
            _logFilePath = null;
        }
    }

    private void Log(LogLevel level, string message)
    {
        var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] |{_name}| {message}";

        if (_logFilePath != null)
        {
            // Write to file
            using var file = new System.IO.StreamWriter(_logFilePath, true);
            file.WriteLine(logMessage);
        }
        else
        {
            // Write to (Godot) console
            GD.Print(logMessage);
        }
    }

    public void Info(string message)
    {
        Log(LogLevel.Info, message);
    }

    public void Debug(string message)
    {
        Log(LogLevel.Debug, message);
    }

    public void Warn(string message)
    {
        Log(LogLevel.Warn, message);
    }

    public void Error(string message)
    {
        Log(LogLevel.Error, message);
    }
}