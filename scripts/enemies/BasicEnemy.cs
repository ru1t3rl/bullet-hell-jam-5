using System;
using System.Threading.Tasks;
using BulletHellJam5.Enemies.Enums;
using BulletHellJam5.Projectiles;
using Godot;

namespace BulletHellJam5.Enemies;

public partial class BasicEnemy : BaseEnemy
{
    [ExportGroup("Projectiles")]
    [Export]
    private PackedScene _projectile;
    [Export]
    private int _numberOfProjectiles;
    [Export]
    private int _timeBetweenShots = 500;
    [Export]
    private float _cooldownAfterShots = 5f;

    [ExportGroup("Movement")]
    [Export]
    private float _maxDestinationRange = 50f;
    [Export]
    private float _rotationSpeed = 2;

    private bool _cooldown = false;
    private Timer _cooldownTimer = new();

    private Vector2? _destination;
    private Vector2 _targetPosition = Vector2.Zero;

    [Export]
    private string _randomSeed = "random_seed";
    private Random _random;

    public override void _Ready()
    {
        base._Ready();

        _random = new Random(_randomSeed.GetHashCode());

        var viewportRect = GetViewportRect();
        _targetPosition = viewportRect.Size / 2;

        AddChild(_cooldownTimer);
        _cooldownTimer.Timeout += FinishCooldown;
        _cooldownTimer.OneShot = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        LookAtVelocityDirection(delta);
    }

    protected override void Move()
    {
        if (_cooldown)
        {
            State = EnemyState.Idle;
            return;
        }


        if (_destination is null && State != EnemyState.Shooting)
        {
            SetDestination();
        }
        else if (_destination is not null)
        {
            var distanceVector = GlobalPosition - _destination.Value;
            float sqrDistance = distanceVector.X * distanceVector.X + distanceVector.Y * distanceVector.Y;
            if (sqrDistance < 1)
            {
                Velocity = Vector2.Zero;
                _destination = null;
                Shoot();
                return;
            }
        }

        if (_destination is null)
        {
            return;
        }

        Velocity = _destination!.Value - GlobalPosition;
        Velocity = Velocity.Normalized();
        Velocity *= _speed;

        State = EnemyState.Moving;
    }

    public async override void Shoot()
    {
        base.Shoot();

        for (int iProjectile = 0; iProjectile < _numberOfProjectiles; iProjectile++)
        {
            var instance = _projectile.Instantiate();
            if (instance is not BaseProjectile projectile)
            {
                GD.PrintErr(
                    "The provided projectile isn't a valid projectile. It doesn't have a script derived from BaseProjectile");

                break;
            }

            AddSibling(instance);
            projectile.Fire(GlobalPosition, _targetPosition - GlobalPosition);

            await Task.Delay(_timeBetweenShots);
        }

        _cooldown = true;
        _cooldownTimer.WaitTime = _cooldownAfterShots;
        _cooldownTimer.Start();
    }

    private void LookAtVelocityDirection(double delta)
    {
        if (State == EnemyState.Moving)
        {
            float targetAngle = float.Atan2(Velocity.Y, Velocity.X);
            Rotation = float.Lerp(Rotation, targetAngle, (float)delta * _rotationSpeed);
        }
        else
        {
            Vector2 direction = _targetPosition - GlobalPosition;
            float targetAngle = float.Atan2(direction.Y, direction.X);
            Rotation = float.Lerp(Rotation, targetAngle, (float)delta * _rotationSpeed);
        }
    }

    private void SetDestination()
    {
        _destination = Position + new Vector2(
            _random.NextSingle() * (_maxDestinationRange * 2) - _maxDestinationRange,
            _random.NextSingle() * (_maxDestinationRange * 2) - _maxDestinationRange
        );

        var viewportSize = GetViewportRect().Size;
        _destination = new Vector2(
            Mathf.Clamp(_destination!.Value.X, 0, viewportSize.X),
            Mathf.Clamp(_destination!.Value.Y, 0, viewportSize.Y)
        );
    }

    private void FinishCooldown()
    {
        _cooldown = false;
        State = EnemyState.Idle;
    }

    protected override void Dispose(bool disposing)
    {
        _cooldownTimer.Timeout -= FinishCooldown;
        base.Dispose(disposing);
    }
}