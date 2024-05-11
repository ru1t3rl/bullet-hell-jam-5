using Godot;

namespace BulletHellJam5.projectiles;

public abstract partial class BaseProjectile : Area2D
{
    [Export]
    private float _speed;

    [Export(hintString: "Lifespan in seconds")]
    private float _lifeSpan = 10;

    [ExportGroup("Enemy Visual")]
    [Export]
    private Sprite2D _sprite;
    [Export]
    private Color _enemyColor = Colors.Red;
    [Export]
    private Color _friendlyColor = Colors.Blue;

    private Vector2 _velocity = Vector2.Zero;

    private Timer _lifespanTimer = new();

    public override void _Ready()
    {
        _lifespanTimer.OneShot = true;
        _lifespanTimer.Timeout += LifespanTimerOnTimeout;
        _sprite ??= GetNode<Sprite2D>(".");
    }

    protected abstract void Move();

    public override void _PhysicsProcess(double delta)
    {
        Move();
        EdgeCheck();
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

    public void Fire(Vector2 direction)
    {
        _velocity = direction.Normalized() * _speed;
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
        float sqrMagnitude = _velocity.X * _velocity.X + _velocity.Y * _velocity.Y;

        if (sqrMagnitude <= _speed * _speed)
        {
            return;
        }

        _velocity = _velocity.Normalized();
        _velocity *= _speed;
    }
}