extends TextureButton


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	button_up.connect(on_up)
	button_down.connect(on_down)
	focus_exited.connect(on_up)

func on_up():
	set_pressed_no_signal(false)

func on_down():
	set_pressed_no_signal(true)
