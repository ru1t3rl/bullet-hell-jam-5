using BulletHellJam5.Projectiles;
using BulletHellJam5.ResourceZones.Resources;
using Godot;

namespace BulletHellJam5.ResourceZones;

public partial class BaseResourceZone : Area2D
{
	[Export]
	private Currency _currency;

	[Export]
	private int _health = 50;
	private int _startHealth;

	[ExportGroup("Resource Generation")]
	[Export]
	private int _numberOfResourcesPerGeneration = 10;
	[Export]
	private float _resourceGenerationInterval = 5f;
	private Timer _resourceGenerationTimer = new();

	private int _currentNumberOfResources;

    private Polygon2D _polygon;
    private TextEdit _healthText;

	public override void _Ready()
	{
        _polygon = GetNode<Polygon2D>("Polygon2D");
        _healthText = GetNode<TextEdit>("HealthText");
		if (_currency is null)
		{
			GD.Print("Linked supply resources isn't a valid supply");
			return;
		}

		SetupTimer();

		_startHealth = _health;
	}


	private void _on_area_entered(Area2D area)
	{
		EmitSignal(nameof(OnCollision), area);
		if (area is not BaseProjectile projectile)
		{
			return;
		}

		TakeDamage(projectile.Damage);
	}

	private void OnBodyEntered(Node2D body)
	{
		EmitSignal(nameof(OnCollision), body);
		if (body is not BaseProjectile projectile)
		{
			return;
		}

		TakeDamage(projectile.Damage);
	}

	public void TakeDamage(int amount)
	{
		_health -= amount;
		EmitSignal(nameof(OnTakeDamage), amount);
        UpdateColor();
        UpdateText();
        HealthCheck();
    }

    //TODO for some reason health is still going below 0
    private void HealthCheck()
    {
		if (_health <= 0)
		{
            _health = 0;
			Die();
		}
	}

	protected virtual void Die()
	{
		EmitSignal(nameof(OnZoneDestroyed), 1);
		QueueFree();
	}

	public void TakeResources(int amount)
	{
		_currentNumberOfResources -= amount;
		EmitSignal(nameof(OnTakeResources), amount);
	}

	public int TakeResourcesHealthBased()
	{
		int resourcesToTake = Mathf.RoundToInt(_currentNumberOfResources * (_health / (float)_startHealth));
		_currentNumberOfResources -= resourcesToTake;

		EmitSignal(nameof(OnTakeResources), 1);
		return resourcesToTake;
	}

	public int TakeAllResources()
	{
		var nResources = _currentNumberOfResources;
		_currentNumberOfResources = 0;

		EmitSignal(nameof(OnTakeResources), nResources);
		return nResources;
	}

	private void SetupTimer()
	{
		AddChild(_resourceGenerationTimer);
		_resourceGenerationTimer.WaitTime = _resourceGenerationInterval;
		_resourceGenerationTimer.Timeout += OnResourceTimerTick;
		_resourceGenerationTimer.Start();
	}

    private void UpdateText()
    {
        _healthText.Text = _health.ToString();
    }

	private void OnResourceTimerTick()
	{
		_currentNumberOfResources += _numberOfResourcesPerGeneration;
		EmitSignal(nameof(OnResourcesGenerated));
    }

    private void UpdateColor()
    {
        if (_polygon == null)
            return;

        // Calculate the new color based on the health
        float healthPercentage = _health / (float)_startHealth;
        Color currentColor = _polygon.Color;

        // Set alpha to 0 if health is less than or equal to 0
        float newAlpha = _health <= 0 ? 0 : currentColor.A;

        Color newColor =
            new Color(1.0f, healthPercentage, healthPercentage, newAlpha); // Preserve or update alpha value

        _polygon.Color = newColor;
	}
}
