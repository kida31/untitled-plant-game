using System.Threading.Tasks;
using Godot;

public partial class SceneTransition : Control
{
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		animationPlayer = GetNode("ColorRect").GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public async Task FadeIn()
	{
		animationPlayer.Play("FadeIn");
		await ToSignal(animationPlayer, "animation_finished");
	}

	public async Task FadeOut()
	{
		animationPlayer.PlayBackwards("FadeIn");
		await ToSignal(animationPlayer, "animation_finished");
	}
}
