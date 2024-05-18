extends CharacterBody2D

var BaseProjectile = load("res://scripts/projectiles/base/BaseProjectile.cs")

func _ready():
	BaseProjectile.Move
	

func _on_basic_projectile_area_entered(area : Area2D):
	if area.is_in_group("Player"):
		BaseProjectile.Reflect
