using Godot;
namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class DetailedWikiController : Node
{
	[Export] private PackedScene _detailedWikiItemView;
	private WikiArticleView _detailedView;

	public override void _Ready()
	{
		EventBus.Instance.OnWikiItemClicked += ShowDetailedWikiView;
	}

	private void ShowDetailedWikiView(ItemStack itemStack)
	{
		foreach (var node in GetChildren())
		{
			node.QueueFree();
		}
		
		_detailedView = _detailedWikiItemView.Instantiate<WikiArticleView>();
		_detailedView.SetItemStack(itemStack);
		
		AddChild(_detailedView);
	}
}
