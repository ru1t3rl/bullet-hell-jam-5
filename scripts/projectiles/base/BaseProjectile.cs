using BulletHellJam5.Enemies;
using Godot;

namespace BulletHellJam5.Projectiles;

public abstract partial class BaseProjectile : Area2D
{
	[Export]
	protected float speed;
	[Export(hintString: "Lifespan in seconds")]
	private float _lifeSpan = 10;
	[Export]
	private int _damage = 1;
	public int Damage => _damage;

	[Export]
	protected ProjectileType _type = ProjectileType.Hostile;
	public ProjectileType Type => _type;

	[ExportGroup("Visual")]
	[Export]
	private Sprite2D _sprite;
	[Export]
	private Color _enemyColor = Colors.Red;
	[Export]
	private Color _friendlyColor = Colors.Blue;

	protected Vector2 velocity = Vector2.Zero;

	private Timer _lifespanTimer = new();

	public override void _Ready()
	{
		AddChild(_lifespanTimer);
		_lifespanTimer.OneShot = true;
		_lifespanTimer.Timeout += LifespanTimerOnTimeout;
		_sprite ??= GetNode<Sprite2D>(".");
	}

	protected abstract void Move(double delta);

	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
		EdgeCheck();
		Rotation = float.Atan2(velocity.Y, velocity.X);
	}

	private void _on_area_2d_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player"))
		{
			_type = ProjectileType.Allied;
			_sprite.Modulate = _friendlyColor;
		}

		EmitSignal(nameof(OnCollision), area);
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			_type = ProjectileType.Allied;
			_sprite.Modulate = _friendlyColor;
		}

		EmitSignal(nameof(OnCollision), body);

		if (_type == ProjectileType.Allied && body is BaseEnemy enemy)
		{
			GD.Print("Enemy got hit");
			enemy.TakeDamage(_damage);
			SetProcess(false);
		}
	}

	private void LifespanTimerOnTimeout()
	{
		Visible = false;
		EmitSignal(nameof(OnLifespanReached));
	}

	public void Fire(Vector2 origin, Vector2 direction)
	{
		GlobalPosition = origin;
		velocity = direction.Normalized() * speed;
		_lifespanTimer.WaitTime = _lifeSpan;
		_lifespanTimer.Start();
	}

	private void EdgeCheck()
	{
		var viewport = GetViewportRect();
		var position = GlobalPosition;

		position.X = position.X switch
		{
			_ when position.X > viewport.Size.X => 0,
			_ when position.X < 0 => viewport.Size.X,
			_ => position.X
		};

		position.Y = position.Y switch
		{
			_ when position.Y > viewport.Size.Y => 0,
			_ when position.Y < 0 => viewport.Size.Y,
			_ => position.Y
		};
	}

	protected void TruncateVelocity()
	{
		float sqrMagnitude = velocity.X * velocity.X + velocity.Y * velocity.Y;

		if (sqrMagnitude <= speed * speed)
		{
			return;
		}

		velocity = velocity.Normalized();
		velocity *= speed;
	}
}
