using BulletHellJam5.projectiles;
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

	public override void _Ready()
	{
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
			GD.Print("Colliding with anything thats not a bullet");
			return;
		}

		TakeDamage(projectile.Damage);
	}

	private void OnBodyEntered(Node2D body)
	{
		EmitSignal(nameof(OnCollision), body);
		if (body is not BaseProjectile projectile)
		{
			GD.Print("Colliding with anything thats not a bullet");
			return;
		}

		TakeDamage(projectile.Damage);
	}

	public void TakeDamage(int amount)
	{
		GD.Print("is this working?");
		_health -= amount;
		EmitSignal(nameof(OnTakeDamage), amount);
		
		if (_health <= 0)
		{
			Die();
		}
	}
	
	protected virtual void Die()
	{

		//EmitSignal(nameof(OnZoneDestroyed));
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

		EmitSignal(nameof(OnTakeResources), resourcesToTake);
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

	private void OnResourceTimerTick()
	{
		_currentNumberOfResources += _numberOfResourcesPerGeneration;
		EmitSignal(nameof(OnResourcesGenerated));
	}
}
