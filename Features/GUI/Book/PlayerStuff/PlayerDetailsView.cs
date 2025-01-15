using Godot;

namespace untitledplantgame.GUI.Book.PlayerStuff;

/// <summary>
///     This class is a control that displays player details. It is visually represented as
///     right hand side of the book.
/// </summary>
public partial class PlayerDetailsView : Control
{
	[Export] private FaithProgressBar _faithProgressBar;
	[Export] private Label label1;
	[Export] private Label label2;
	[Export] private Label label3;
	[Export] private Label label4;

	public override void _Ready()
	{
		UpdateGold(0);
		UpdateItemsSold(0);
		UpdatePlantsHarvested(0);
		UpdatePlantsDied(0);
		UpdateFaith(0);
	}
	
	public void UpdateGold(int value)
	{
		label1.Text = $"Total Gold: {value}";
	}
	
	public void UpdateItemsSold(int value)
	{
		label2.Text = $"Items Sold: {value}";
	}
	
	public void UpdatePlantsHarvested(int value)
	{
		label3.Text = $"Plants harvested: {value}";
	}
	
	public void UpdatePlantsDied(int value)
	{
		label4.Text = $"Plants died: {value}";
	}
	
	public void UpdateFaith(float value)
	{
		_faithProgressBar.Value = value;
	}
}
