shader_type canvas_item;

const float speed = 0.5;
uniform sampler2D noise_texture : repeat_enable;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec2 uv_update = vec2(UV.x + TIME * speed, UV.y);
	COLOR = texture (noise_texture, uv_update);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
