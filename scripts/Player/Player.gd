extends CharacterBody2D

#5 to me seems like a good max speed. Testing required
@export var SPEED: float = 1
#Use acceleration and deceleration for momentum maybe?
#@export var ACCELERATION : float = 1.2
#@export var DECCELERATION : int = -ACCELERATION

#Should be replaced with planet center position
var Planet_Center_Position : Vector2 = Vector2(1100, 900)
var previous_angle = 0

func _physics_process(delta):
	var input_direction = get_input_direction()
	var angle = get_angle()
	var angle_difference = angle - previous_angle
	
	if input_direction == 1:
		rotate_around_point(Planet_Center_Position, SPEED * delta)
		rotate(angle_difference)
	elif input_direction == -1:
		rotate_around_point(Planet_Center_Position, -SPEED * delta)
		rotate(angle_difference)
		
	previous_angle = angle
	move_and_slide()

func get_input_direction():
	return int(Input.is_action_pressed("Rotate_Clockwise")) - int(Input.is_action_pressed("Rotate_AntiClockwise"))

func get_angle():
	return atan2(self.position.y - Planet_Center_Position.y, self.position.x - Planet_Center_Position.x)

func rotate_around_point(point: Vector2, angle: float):
	var offset = self.position - point
	var rotated_offset = offset.rotated(angle)
	self.position = point + rotated_offset