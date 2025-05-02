using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;

[Tool]
public partial class ResourceRemapButton : Button
{
    public ResourceRemapButton()
    {
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        PromptConfirm("Overwrite Remaps", "This process will remove existing remaps. Continue?", ImportRemaps);
    }

    private void PromptConfirm(string title, string text, Action callback)
    {
        var dialog = new ConfirmationDialog()
        {
            Title = title,
            DialogText = text,
        };
        dialog.Confirmed += callback;

        AddChild(dialog);
        dialog.PopupCentered();
        dialog.Show();
    }

    // There are multiple Task.Delay() to give GUI time to render. LF> alternative
    private async void ImportRemaps()
    {
        var processWindow = new ProcessWindow();
        processWindow.Title = "Add resource remaps";
        processWindow.DialogText = "Log:";
        AddChild(processWindow);
        processWindow.GetOkButton().Disabled = true;
        processWindow.PopupCentered(new(360, 240));
        processWindow.Show();

        // Collect .tres
        processWindow.PrintLine("Collecting '.tres' files...");
        await Task.Delay(1); // idk how to wait
        var resources = GetFilesRecursively("res://")
            .Where(fileName => fileName.EndsWith(".tres"))
            .ToList();
        processWindow.PrintLine($"  Found {resources.Count} '.tres' files.");

        // Filter by _DE.tres
        processWindow.PrintLine("Collecting DE resources...");
        var resourceMap = new Dictionary<string, string>();
        var deResources = resources.Where(res => res.EndsWith("_DE.tres")).ToFrozenSet();
        processWindow.PrintLine($"  Found {deResources.Count} resources ending with '_DE.tres'.");

        // Map to .tres
        processWindow.PrintLine($"Verifying corresponding main resources...");
        foreach (var res in resources.Where(res => !res.Contains("_DE.tres")))
        {
            var deName = res.Replace(".tres", "_DE.tres");
            if (deResources.Contains(deName))
            {
                resourceMap.Add(res, deName);
            }
        }
        foreach (var deRes in deResources)
        {
            if (!resourceMap.ContainsValue(deRes))
            {
                processWindow.PrintLine($"  Could not find main resource for '{deRes}'");
                GD.PushError($"Could not find main resource for :'{deRes}'");
            }
        }


        // Save Changes after confirmation
        async Task SaveStuff_()
        {
            processWindow.PrintLine("Preparing remap dictionary...");
            await Task.Delay(1); // idk how to wait

            // Godot-ify
            var godotRemaps = new Godot.Collections.Dictionary<string, string[]>();
            foreach (var (en, de) in resourceMap)
            {
                var deFormatted = de + ":de";
                godotRemaps.Add(en, [deFormatted]);
                processWindow.PrintLine($"  {en} -> {deFormatted}");
                await Task.Delay(1); // idk how to wait
            }

            SetRemaps(godotRemaps);
            ProjectSettings.Save();
            processWindow.PrintLine("\nRemap overwritten.");
            processWindow.GetOkButton().Disabled = false;
        }

        if (resourceMap.Count == deResources.Count)
        {
            SaveStuff_();
        }
        else
        {
            PromptConfirm("Confirm Missing", $"Found {deResources.Count} DE resources, but only {resourceMap.Count} EN resources. Continue?", () => SaveStuff_());
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

    private static Godot.Collections.Dictionary<string, string[]> GetRemaps()
    {
        return ProjectSettings.GetSetting("internationalization/locale/translation_remaps").AsGodotDictionary<string, string[]>();
    }

    private static void SetRemaps(Godot.Collections.Dictionary<string, string[]> remaps)
    {
        ProjectSettings.SetSetting("internationalization/locale/translation_remaps", remaps);
    }
}
