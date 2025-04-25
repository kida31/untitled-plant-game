using System;
using System.Collections.Generic;
using Godot;

namespace untitledplantgame.GUI.HUDs.SettingsMenu;

/// <summary>
///		Language selection dropdown. Not a very flexible class, but should be sufficient for re-use.
///		Attach this to a Node>Control>OptionButton in the scene tree. The options will be appended automatically.
/// </summary>
public partial class LanguageOption : OptionButton
{
	private readonly Dictionary<int, Tuple<string, string>> _languages = new()
	{
		{0, new Tuple<string, string>("English", "en")},
		{1, new Tuple<string, string>("Deutsch", "de")},
	};

	public override void _Ready()
	{
		var currentLocale = TranslationServer.GetLocale();
		foreach (var (index, (label, locale)) in _languages)
		{
			AddItem(label, index);

			if (locale == currentLocale)
			{
				Select(index);
			}
		}

		ItemSelected += OnItemSelected;
	}

	private void OnItemSelected(long index)
	{
		var idx = (int) (index % _languages.Count);
		var locale = _languages[idx].Item2;
		TranslationServer.SetLocale(locale);
	}
}
