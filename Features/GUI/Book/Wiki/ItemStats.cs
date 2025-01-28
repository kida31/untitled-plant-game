using System.Linq;
using System.Text.RegularExpressions;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Item.Components;

namespace untitledplantgame.GUI.Book.Wiki;

public partial class ItemStats : Control
{
	[ExportGroup("Node Setup")] [Export] private RichTextLabel _name;
	[Export] private RichTextLabel _tags;
	[Export] private RichTextLabel _price;
	[Export] private RichTextLabel _harvestResult;
	[Export] private RichTextLabel _medicineEffectsPositives;
	[Export] private RichTextLabel _medicineEffectsNegatives;

	private static readonly Texture2D CoinIcon = GD.Load<Texture2D>(BbImage.Coin.Path);
	public IItemStack Item { get; private set; }

	public void SetItem(IItemStack item)
	{
		Clear();
		Item = item;
		if (item == null) return;

		// Name
		_name.PushParagraph(HorizontalAlignment.Center);
		_name.AppendText(item.Name);
		
		// Category
		// _name.AppendText(" - ");
		_name.PushColor(Colors.Brown);
		_name.AppendText($" [{item.Category.Name}]");
		_name.Pop();
		_name.Pop();

		// Tags
		var tags = item.GetComponent<TagsComponent>();
		if (tags != null)
		{
			_tags.PushParagraph(HorizontalAlignment.Center);
			_tags.PushColor(Colors.DarkGray);
			var tagNames = tags
				.Select(t => Regex.Replace(t.ToString(), "^Is", "")) // Remove "Is" prefix from tags.
				.Select(t => t.ToLower()) // Lowercase tags.
				.Select(t => $"#{t}"); // Add hashtag to tags.
			_tags.AppendText(string.Join("  ", tagNames));
			_tags.Pop();
			_tags.Pop();
		}

		// Price
		_price.PushParagraph(HorizontalAlignment.Center);
		_price.AppendText(item.BaseValue.ToString());
		_price.AddImage(CoinIcon);
		_price.Pop();

		// Harvestable - nothing to show here yet

		// Medicine Effects
		var medicineEffects = item.GetComponent<MedicineComponent>();
		if (medicineEffects != null)
		{
			_medicineEffectsPositives.PushParagraph(HorizontalAlignment.Center);
			foreach (var (effect, value) in medicineEffects.TheGoodStuff)
			{
				_medicineEffectsPositives.PushColor(Colors.MediumSpringGreen);
				_medicineEffectsPositives.AppendText($"+{value} {effect}\n");
				_medicineEffectsPositives.Pop();
			}
			_medicineEffectsPositives.Pop();

			_medicineEffectsNegatives.PushParagraph(HorizontalAlignment.Center);
			foreach (var (effect, value) in medicineEffects.TheBadStuff)
			{
				_medicineEffectsNegatives.PushColor(Colors.IndianRed);
				_medicineEffectsNegatives.AppendText($"+{value} {effect}\n");
				_medicineEffectsNegatives.Pop();
			}
			_medicineEffectsNegatives.Pop();
		}
	}

	public void Clear()
	{
		// Clear all fields
		_name.Text = "";
		_tags.Text = "";
		_price.Text = "";
		_harvestResult.Text = "";
		_medicineEffectsPositives.Text = "";
		_medicineEffectsNegatives.Text = "";
	}
}
