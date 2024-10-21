extends Node2D

@onready var interaction_area: InteractionArea = $InteractionArea

var shop_scene = load("res://seed_shop/seed_shop.tscn")
var shop_instance = null

func _input(event):
	if Input.is_action_just_pressed("open_shop"):
		if shop_instance == null:
			print(typeof(shop_scene))  # This should print "17" if it's a PackedScene
			open_shop()

func open_shop():
	shop_instance = shop_scene.instantiate()  # Create an instance of the shop scene
	add_child(shop_instance)  # Add the shop instance to the current scene
	print("Shop opened")

func close_shop():
	if shop_instance != null:
		shop_instance.queue_free()
		shop_instance = null
		print("Shop closed")

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	interaction_area.interact = Callable(self, "_on_interact")

func _on_interact():
	open_shop()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
