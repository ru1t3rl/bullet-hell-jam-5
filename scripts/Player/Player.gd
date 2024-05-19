extends CharacterBody2D

#5 to me seems like a good max speed. Testing required
@export var SPEED: float = 1
#Use acceleration and deceleration for momentum maybe?
#@export var ACCELERATION : float = 1.2
#@export var DECCELERATION : int = -ACCELERATION

#Should be replaced with planet center position
@export var Planet_Center_Position : Vector2 = Vector2(650, 650)
var previous_angle = 0

func _physics_process(delta):
	var input_direction = get_input_direction()
	var angle = get_angle()
	var angle_diff = angle - previous_angle
	
	if input_direction == 1:
		rotate_around_point(Planet_Center_Position, SPEED * delta)
		rotate(angle_diff)
	elif input_direction == -1:
		rotate_around_point(Planet_Center_Position, -SPEED * delta)
		rotate(angle_diff)
		
	previous_angle = angle
	move_and_slide()

func get_input_direction():
	return int(Input.is_action_pressed("Rotate_Clockwise")) - int(Input.is_action_pressed("Rotate_AntiClockwise"))

func get_angle():
	#Returns the angle (in radians) between the line connecting the player's position and the center of the planet and the x-axis.
	#The angle is measured counterclockwise from the positive x-axis.
	return atan2(self.position.y - Planet_Center_Position.y, self.position.x - Planet_Center_Position.x)

func rotate_around_point(point: Vector2, angle: float):
	var offset = self.position - point
	var rotated_offset = offset.rotated(angle)
	self.position = point + rotated_offset

<<<<<<< Updated upstream
func _on_area_2d_area_entered(area):
	#var Incident : Vector2 = Vector2.ZERO
	#var Normal : Vector2 = Vector2(self.position.x - Planet_Center_Position.x, self.position.y - Planet_Center_Position.y).normalized()
	#var Reflected : Vector2 = Vector2.ZERO
	
	
	pass
	#print("Collided")
=======
signal ProjectileDetected

func GetNormal() :
	return Vector2(position.x - Planet_Center_Position.x, position.y - Planet_Center_Position.y).normalized()

func _on_area_2d_area_entered(area : Area2D):
	if area.is_in_group("Projectile"):
		#print("Area Entered")
		emit_signal("ProjectileDetected", GetNormal())
>>>>>>> Stashed changes
