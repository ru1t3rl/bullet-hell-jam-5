using BulletHellJam5.Enemies;
using Godot;

namespace BulletHellJam5.Projectiles;

public abstract partial class BaseProjectile : Area2D
{
    [Export]
    protected float Speed;

    [Export]
    private float _rotationSpeed = 5;

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

    protected Vector2 Velocity = Vector2.Zero;

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
        LookAtVelocity(delta);
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

    protected void OnBodyEntered(Node2D body)
    {
        OnCollisionWithBody(body);
    }

    protected virtual void OnCollisionWithBody(Node2D body)
    {
        var colliderIsPlayer = body.IsInGroup("Player");
        if (colliderIsPlayer)
        {
            _type = ProjectileType.Allied;
            _sprite.Modulate = _friendlyColor;
        }

        EmitSignal(nameof(OnCollision), body);

        if (_type == ProjectileType.Allied && body is BaseEnemy enemy)
        {
            enemy.TakeDamage(_damage);
            SetProcess(false);
        }

        if (!colliderIsPlayer)
        {
            QueueFree();
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
        Velocity = direction.Normalized() * Speed;
        _lifespanTimer.WaitTime = _lifeSpan;
        _lifespanTimer.Start();
    }

    protected void LookAtVelocity(double delta)
    {
        float targetAngle = float.Atan2(Velocity.Y, Velocity.X);
        Rotation = float.Lerp(Rotation, targetAngle, (float)delta * _rotationSpeed);
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
        float sqrMagnitude = Velocity.X * Velocity.X + Velocity.Y * Velocity.Y;

        if (sqrMagnitude <= Speed * Speed)
        {
            return;
        }

        Velocity = Velocity.Normalized();
        Velocity *= Speed;
    }
}