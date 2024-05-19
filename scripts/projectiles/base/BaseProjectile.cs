using Godot;

namespace BulletHellJam5.projectiles;

public abstract partial class BaseProjectile : CharacterBody2D
{
	[Export]
	protected float speed;
	[Export(hintString: "Lifespan in seconds")]
	private float _lifeSpan = 10;
	[Export]
	private int _damage = 1;
	public int Damage => _damage;

	[ExportGroup("Enemy Visual")]
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
<<<<<<< Updated upstream
	
	public virtual void Reflect(double delta)
	{
		KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);
		if (collision != null)
		{
			var reflect = collision.GetRemainder().Bounce(collision.GetNormal());
			velocity = velocity.Bounce(collision.GetNormal());
			MoveAndCollide(reflect);
		}
	}
=======
>>>>>>> Stashed changes

	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
		EdgeCheck();
		Rotation = float.Atan2(velocity.Y, velocity.X);
	}

	private void OnBodyEntered(Node2D body)
	{
		EmitSignal(nameof(OnCollision), body);

		// TODO [LR]: When player is in game implement collision check to change color
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
