using Godot;
using System;

public abstract partial class BaseEnemy : CharacterBody2D
{
	[Export]
	protected float _speed;
	protected Vector2 velocity;

	[Export(PropertyHint.Range)]
	protected float powerupDropchange = 0.5f;

	protected Action OnShoot;
	
	protected abstract void Move();
	public abstract void Shoot();

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}
}
