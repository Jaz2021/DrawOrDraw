extends Sprite2D

var colorDict = {
	"red" : "850000ff",
	"blue" : "2c00c4ff",
	"green" : "00a130ff",
	"yellow" : "a5a500ff",
	"purple" : "9e5affff",
	"pink" : "a60080ff",
	"anotherBlue" : "009595ff"
}

func _ready() -> void:
	var keys = colorDict.keys()
	var color = keys.pick_random()
	self_modulate = Color(colorDict[color])
