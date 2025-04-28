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
		if (OS.IsDebugBuild())
		{
			// For testing purposes
			AddMoreLanguages();
		}

		var currentLocale = TranslationServer.GetLocale();
		GD.Print("My locale=" + currentLocale);
		foreach (var (index, (label, locale)) in _languages)
		{
			AddItem(label, index);

			if (TranslationServer.CompareLocales(locale, currentLocale) > 4)
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

	private void AddMoreLanguages()
	{
		var otherLanguages = new Dictionary<int, Tuple<string, string>>
		{
			{2, new Tuple<string, string>("Français", "fr")},
			{3, new Tuple<string, string>("Español", "es")},
			{4, new Tuple<string, string>("Italiano", "it")},
			{5, new Tuple<string, string>("Português (BR)", "pt_BR")},
			{6, new Tuple<string, string>("Português (PT)", "pt")},
			{7, new Tuple<string, string>("Русский", "ru")},
			{8, new Tuple<string, string>("Ελληνικά", "el")},
			{9, new Tuple<string, string>("Türkçe", "tr")},
			{10, new Tuple<string, string>("Dansk", "da")},
			{11, new Tuple<string, string>("Norsk Bokmål", "no")},
			{12, new Tuple<string, string>("Svenska", "sv")},
			{13, new Tuple<string, string>("Nederlands", "nl")},
			{14, new Tuple<string, string>("Polski", "pl")},
			{15, new Tuple<string, string>("Suomi", "fi")},
			{16, new Tuple<string, string>("日本語", "ja")},
			{17, new Tuple<string, string>("简体中文", "zh_CN")},
			{18, new Tuple<string, string>("繁體中文", "zh_TW")},
			{19, new Tuple<string, string>("한국어", "ko")},
		};

		foreach (var (index, (label, locale)) in otherLanguages)
		{
			_languages.Add(index, new Tuple<string, string>(label, locale));
		}
	}
}
