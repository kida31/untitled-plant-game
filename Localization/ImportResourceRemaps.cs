using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using untitledplantgame.Common;

public partial class ImportResourceRemaps : Control
{
    [Export(PropertyHint.Dir)] private string _dir;
    public override void _Ready()
    {
        GetNode<Button>("Button").Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        var remaps = GetRemaps();
        if (remaps.Count == 0)
        {
            ImportRemaps();
        }
        else
        {
            PromptAccept("Overwrite Remaps", "This process will remove existing remaps. Continue?", ImportRemaps);
        }
    }

    private void PromptAccept(string title, string text, Action callback)
    {
        var dialog = new AcceptDialog()
        {
            Title = title,
            DialogText = text,
        };
        dialog.Confirmed += callback;

        AddChild(dialog);
        dialog.PopupCentered();
        dialog.Show();
    }

    private void ImportRemaps()
    {
        GD.Print("Preparing...");
        var resources = GetFilesRecursively("res://")
            .Where(fileName => fileName.EndsWith(".tres"))
            .ToList();

        var resourceMap = new System.Collections.Generic.Dictionary<string, string>();
        var deResources = resources.Where(res => res.EndsWith("_DE.tres")).ToFrozenSet();
        foreach (var res in resources.Where(res => !res.Contains("_DE.tres")))
        {
            var deName = res.Replace(".tres", "_DE.tres");
            if (deResources.Contains(deName))
            {
                resourceMap.Add(res, deName);
            }
        }

        // Verify missing
        foreach (var deRes in deResources)
        {
            if (!resourceMap.ContainsValue(deRes))
            {
                GD.PushError($"Did not find main resource for :'{deRes}'");
            }
        }

        void SaveStuff_()
        {
            GD.Print("Persisting...");

            // Godot-ify
            var godotRemaps = new Godot.Collections.Dictionary<string, string[]>();
            foreach (var (en, de) in resourceMap)
            {
                var deFormatted = de + ":de";
                godotRemaps.Add(en, [deFormatted]);
            }

            SetRemaps(godotRemaps);
            ProjectSettings.Save();
            GD.Print("Saved.");
        }

        if (resourceMap.Count == deResources.Count)
        {
            SaveStuff_();
        }
        else
        {
            PromptAccept("Confirm Missing", $"Found {deResources.Count} DE resources, but only {resourceMap.Count} EN resources. Continue?", SaveStuff_);
        }
    }

    private List<string> GetFilesRecursively(string dirPath)
    {
        var dir = DirAccess.Open(dirPath);
        if (dir == null)
        {
            GD.PushError(DirAccess.GetOpenError());
            GD.PushError($"No directory for '{dirPath}'");
            return [];
        }
        var localFiles = dir.GetFiles();
        var files = localFiles.Select(f => Path.Join(dirPath, f)).ToList();
        var directories = dir.GetDirectories();

        foreach (var d in directories)
        {
            files.AddRange(GetFilesRecursively(Path.Join(dirPath, d)));
        }

        return files;
    }

    private Godot.Collections.Dictionary<string, string[]> GetRemaps()
    {
        return ProjectSettings.GetSetting("internationalization/locale/translation_remaps").AsGodotDictionary<string, string[]>();
    }

    private void SetRemaps(Godot.Collections.Dictionary<string, string[]> remaps)
    {
        ProjectSettings.SetSetting("internationalization/locale/translation_remaps", remaps);
    }
}
