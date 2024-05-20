using BulletHellJam5.Enemies.Enums;
using Godot;

namespace BulletHellJam5.Enemies;

public abstract partial class BaseEnemy : CharacterBody2D
{
    protected ScoreAdjuster _scoreAdjuster;

    [Export]
    public ScoreAdjuster ScoreAdjuster
    {
        get => _scoreAdjuster ??= FindScoreAdjuster();
        set
        {
            _scoreAdjuster = value;
            UpdateConfigurationWarnings();
        }
    }

    [Export]
    protected float _health;
    public float Health => _health;

    [Export]
    protected int _score;
    public int Score => _score;

    [Export]
    protected float _speed;

    [Export(PropertyHint.Range, "0, 1, 0.1f")]
    protected float _powerupDropchange = 0.5f;

    private EnemyState _state = EnemyState.Idle;

    public EnemyState State
    {
        get => _state;
        protected set
        {
            _state = value;
            EmitSignal(nameof(OnStateChange), (int)_state);
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
        EmitSignal(nameof(OnShoot));
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        EmitSignal(nameof(OnTakeDamage), Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        State = EnemyState.Dead;
        ScoreAdjuster?.AdjustScore(Score);
        EmitSignal(nameof(OnDie), this);
        QueueFree();
    }

    private ScoreAdjuster FindScoreAdjuster()
    {
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is ScoreAdjuster score)
            {
                return score;
            }
        }

        return null;
    }

    public override string[] _GetConfigurationWarnings()
    {
        var scoreAdjuster = FindScoreAdjuster();
        if (scoreAdjuster is null)
        {
            return ["This node must have a ScoreAdjuster child"];
        }

        return [];
    }
}