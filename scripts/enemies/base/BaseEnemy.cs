using BulletHellJam5.Enemies.Enums;
using Godot;

namespace BulletHellJam5.Enemies;

public abstract partial class BaseEnemy : CharacterBody2D
{
    [Export]
    protected float _health;
    public float Health => _health;

    [Export]
    protected float _speed;
    protected Vector2 _velocity = Vector2.Zero;

    [Export(PropertyHint.Range, "0, 1, 0.1f")]
    protected float _powerupDropchange = 0.5f;

    private EnemyState _state = EnemyState.Idle;

    public EnemyState State
    {
        get => _state;
        protected set
        {
            _state = value;
            EmitSignal(nameof(OnStateChangeEventHandler), (int)_state);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Move();
        MoveAndSlide();
    }

    protected abstract void Move();

    public virtual void Shoot()
    {
        State = EnemyState.Shooting;
        EmitSignal(nameof(OnShootEventHandler));
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        EmitSignal(nameof(OnTakeDamage), Health);

        if (_health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        State = EnemyState.Dead;
        EmitSignal(nameof(OnDieEventHandler), this);
    }
}