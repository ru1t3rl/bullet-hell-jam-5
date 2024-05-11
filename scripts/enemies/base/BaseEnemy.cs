using Godot;

namespace BulletHellJam5.Enemies;

public abstract partial class BaseEnemy : CharacterBody2D
{
    [Export]
    protected float _health;

    public float Health => _health;

    [Export]
    protected float _speed;

    protected Vector2 _velocity;

    [Export(PropertyHint.Range, "0, 1, 0.1f")]
    protected float _powerupDropchange = 0.5f;

    protected abstract void Move();

    public virtual void Shoot()
    {
        EmitSignal(nameof(OnShootEventHandler));
    }

    public override void _PhysicsProcess(double delta)
    {
        Move();
        MoveAndSlide();
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        EmitSignal(nameof(OnTakeDamage), Health);
    }
}