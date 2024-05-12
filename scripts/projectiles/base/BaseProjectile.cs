using Godot;

namespace BulletHellJam5.projectiles;

public abstract partial class BaseProjectile : Area2D
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
        _lifespanTimer.OneShot = true;
        _lifespanTimer.Timeout += LifespanTimerOnTimeout;
        _sprite ??= GetNode<Sprite2D>(".");
    }

    protected abstract void Move(double delta);

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
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
        velocity = direction.Normalized() * speed;
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