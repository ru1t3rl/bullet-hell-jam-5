using System;
using BulletHellJam5.Enemies.Enums;
using BulletHellJam5.projectiles;
using Godot;

namespace BulletHellJam5.Enemies;

public partial class BasicEnemy : BaseEnemy
{
    [ExportGroup("Projectiles")]
    [Export]
    private PackedScene _projectile;
    [Export]
    private int _numberOfProjectiles;

    [ExportGroup("Movement")]
    [Export]
    private float _cooldownAfterShot = 5f;
    [Export]
    private float _maxDestinationRange = 50f;

    private bool _cooldown = false;
    private Timer _cooldownTimer = new();

    private Vector2? _destination = null;

    [Export]
    private string _randomSeed = "random_seed";
    private Random _random;

    public override void _Ready()
    {
        base._Ready();

        _random = new Random(_randomSeed.GetHashCode());

        _cooldownTimer.Timeout += FinishCooldown;
        _cooldownTimer.OneShot = true;
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
                Shoot();
                return;
            }
        }

        Velocity = Position - _destination!.Value;
        Velocity = Velocity.Normalized();
        Velocity *= _speed;

        State = EnemyState.Moving;
    }

    public override void Shoot()
    {
        Vector2 viewportSize = GetViewportRect().Size;
        Vector2 target = new Vector2(viewportSize.X / 2, viewportSize.Y / 2);
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
            projectile.Fire(GlobalPosition - target);
        }

        base.Shoot();
        _cooldown = true;
        _cooldownTimer.Start();
    }

    private void SetDestination()
    {
        _destination = Position + new Vector2(
            _random.NextSingle() * (_maxDestinationRange * 2) - _maxDestinationRange,
            _random.NextSingle() * (_maxDestinationRange * 2) - _maxDestinationRange
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