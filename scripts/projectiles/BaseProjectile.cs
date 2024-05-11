using Godot;

namespace BulletHellJam5.projectiles;

public partial class BaseProjectile : Area2D
{
    [Export]
    private float _speed;

    [Export(hintString: "Lifespan in seconds")]
    private float _lifeSpan = 10;

    private Vector2 _velocity = Vector2.Zero;

    private Timer _lifespanTimer = new();

    public override void _Ready()
    {
        _lifespanTimer.Timeout += LifespanTimerOnTimeout;
    }

    private void LifespanTimerOnTimeout()
    {
        Visible = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += _velocity * (float)delta;
        EdgeCheck();
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