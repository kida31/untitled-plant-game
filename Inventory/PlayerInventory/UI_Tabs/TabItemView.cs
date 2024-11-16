using System.Linq;
using Godot;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;


/*
 * The idea was to have a script that can be used to easily change up the UI and as long as a GridContainer exists to display items in it,
 * the user can make and break the UI however they see fit.
 *
 * But Script Execution Order also messes this up. This means when we return the GridContainer of the itemView for the first time, we need
 * to assign it. Since this should be done only once, we will ignore the assignment as soon as GridContainer is not null anymore.
 */
public partial class TabItemView : Control
{
	private GridContainer _gridContainer;

	public GridContainer GetGridContainer()
	{
		return _gridContainer ??= FindFirstGridContainer(this);
	}
	
	private GridContainer FindFirstGridContainer(Node parent)
	{
		if (parent is GridContainer gridContainer)
		{
			return gridContainer;
		}

		return parent.GetChildren().Select(FindFirstGridContainer).FirstOrDefault(found => found != null);
	}
}
