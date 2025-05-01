using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.ExtensionMethods;

namespace untitledplantgame.GUI.HUDs;

/// <summary>
///		This class is a container for MiniNotification.
///		It handles spawning logic as well as animation for each notification.
///		Band-aid: This class automatically subscribes to the EventBus to show notifications for item pick ups.
/// </summary>
public partial class NotificationBox : Control
{
	// Please change the values in the inspector, unless these do not make sense at all.
	private const float DefaultDuration = 2f;
	private const float DefaultFadeInDuration = 0.5f;
	private const float DefaultFadeOutDuration = 2f;
	
	[Export] public float Duration = DefaultDuration;
	[Export] public float FadeInDuration = DefaultFadeInDuration;
	[Export] public float FadeOutDuration = DefaultFadeOutDuration;
	
	[ExportGroup("Setup")]
	[Export] private PackedScene _notificationScene;
	[Export] private Control _container;
	
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);

		// I dont know where to put this. It seems silly to have class define the general behaviour of the class :/
		// Good luck, next person.
		EventBus.Instance.OnItemAddedToInventory += (item) =>
		{
			AddNotification($"+{item.Amount} {Tr(item.Name)}", item.Icon);
		};
	}
	
	private void AddNotification(string text, Texture2D texture = null)
	{
		// Create a new notification instance
		var notification = _notificationScene.Instantiate<MiniNotification>();

		notification.Texture = texture;
		notification.Text = text;
		notification.VScale = 0;

		// Start the fade-in and fade-out animations
		var tween = CreateTween();
		tween.Parallel().TweenProperty(notification, "VScale", 1.0f, FadeInDuration);
		tween.TweenInterval(Duration);
		tween = notification.FadeOut(FadeOutDuration, tween);

		ToSignal(tween, Tween.SignalName.Finished).OnCompleted(() =>
		{
			_logger.Debug(
				$"{notification.Text}: finished scale={notification.VScale}, size={notification.Content.Size}, modulate={notification.Modulate}");
			notification.QueueFree();
		});

		// Add the notification to the scene
		_container.AddChild(notification);
	}
}
