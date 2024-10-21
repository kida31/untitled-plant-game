extends Control

var player_currency = 100

var seed_data = {
	"Carrot": {"price": 10, "available": 5, "owned": 0},
	"Tomato": {"price": 15, "available": 3, "owned": 0},
	"Pumpkin": {"price": 20, "available": 2, "owned": 0}
}

func _on_close_button_pressed() -> void:
	get_parent().close_shop()

func _on_seed_button_pressed(seed_type) -> void:
	var seed_info = seed_data[seed_type]
	if seed_info["available"] > 0 and player_currency >= seed_info["price"]:
		player_currency -= seed_info["price"]
		seed_info["available"] -= 1
		seed_info["owned"] += 1
		
		update_seed_labels(seed_type)
		update_currency_label()
		print("Bought a " + seed_type + " seed. Total Owned: " + str(seed_info["owned"]))
	else:
		print("Not enough seeds available or not enough currency!")

func _on_carrot_button_pressed():
	_on_seed_button_pressed("Carrot")

func _on_tomato_button_pressed() -> void:
	_on_seed_button_pressed("Tomato")

func _on_pumpkin_button_pressed() -> void:
	_on_seed_button_pressed("Pumpkin")

func update_currency_label():
	$ColorRect/money.text = "Money: " + str(player_currency)

func update_seed_labels(seed_type):
	match seed_type:
		"Carrot":
			$ColorRect/GridContainer/Carrots/available.text = "Available: " + str(seed_data["Carrot"].available)
			$ColorRect/GridContainer/Carrots/owned.text = "Owned: " + str(seed_data["Carrot"].owned)
			$ColorRect/GridContainer/Carrots/price.text = "Price: " + str(seed_data["Carrot"].price)
		"Tomato":
			$ColorRect/GridContainer/Tomatoes/available.text = "Available: " + str(seed_data["Tomato"].available)
			$ColorRect/GridContainer/Tomatoes/owned.text = "Owned: " + str(seed_data["Tomato"].owned)
			$ColorRect/GridContainer/Tomatoes/price.text = "Price: " + str(seed_data["Tomato"].price)
		"Pumpkin":
			$ColorRect/GridContainer/Pumpkins/available.text = "Available: " + str(seed_data["Pumpkin"].available)
			$ColorRect/GridContainer/Pumpkins/owned.text = "Owned: " + str(seed_data["Pumpkin"].owned)
			$ColorRect/GridContainer/Pumpkins/price.text = "Price: " + str(seed_data["Pumpkin"].price)


func _ready():
	update_seed_labels("Carrot")
	update_seed_labels("Tomato")
	update_seed_labels("Pumpkin")
	update_currency_label()
