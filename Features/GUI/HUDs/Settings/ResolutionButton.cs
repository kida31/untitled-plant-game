using Godot;

public partial class ResolutionButton : OptionButton
{
	public override void _Ready()
	{
		// We are not doing that right now. 
		
		/*
		 * What I should do is change the visual Resolutions provided in the engine, to code-based solutions.
		 *
		 * > Create Vector2 that represents resolution
		 * > Save them in a dictionary with resolution and text { Vector2(1920, 1080), "1920x1080" }
		 * > Iterate through the list and create the Items as Options (this.AddItem())
		 * > Whenever the page is loaded again through "Settings" Button, compare with dictionary => Vector correlates to text => to ItemText!
		 */
		
	}
}
