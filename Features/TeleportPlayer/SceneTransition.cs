using System.Threading.Tasks;
using Godot;

// TODO namespace
public partial class SceneTransition : Control
{
	public static SceneTransition Instance { get; private set; }
	
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
			return;
		}
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
