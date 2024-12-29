using Godot;

public partial class PlayerDetailsView : Control
{
	[Export] private FaithProgressBar _faithProgressBar;
	[Export] private Label label1;
	[Export] private Label label2;
	[Export] private Label label3;
	[Export] private Label label4;

	public override void _Ready() {
		SetPlayerDetails(null);
	}

	public void SetPlayerDetails(object obj)
	{
		label1.Text = "Total Gold: 0";
		label2.Text = "Items Sold: 0";
		label3.Text = "Plants harvested: 0";
		label4.Text = "Plants died: 0";

		_faithProgressBar.Value = 2.66;
	}
}
