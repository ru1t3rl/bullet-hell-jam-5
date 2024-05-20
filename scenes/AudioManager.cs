using Godot;

public partial class AudioManager : Node2D
{
	private static AudioManager _instance;

	public static AudioManager Instance
	{
		get => _instance;
		set => _instance = value;
	}

	public AudioStreamPlayer2D musicPlayer, laserSfx, enemyDeath, planetSfx, zoneSfx, bounce;

	public override void _Ready()
	{
		InstanceCheck();
		musicPlayer = GetNode<AudioStreamPlayer2D>("MainTheme");
		laserSfx = GetNode<AudioStreamPlayer2D>("EnemyLaser");
		enemyDeath = GetNode<AudioStreamPlayer2D>("EnemyDeath");
		planetSfx = GetNode<AudioStreamPlayer2D>("PlanetDestroyed");
		zoneSfx = GetNode<AudioStreamPlayer2D>("ZoneDestroyed");
		bounce = GetNode<AudioStreamPlayer2D>("Bounce");

		musicPlayer.Stream = GD.Load<AudioStream>("res://assets/audio/ExplorationTheme.wav");
		laserSfx.Stream = GD.Load<AudioStream>("res://assets/audio/laserRetro_000.ogg");
		enemyDeath.Stream = GD.Load<AudioStream>("res://assets/audio/explosionCrunch_001.ogg");
		planetSfx.Stream = GD.Load<AudioStream>("res://assets/audio/explosionCrunch_001.ogg");
		zoneSfx.Stream = GD.Load<AudioStream>("res://assets/audio/explosionCrunch_001.ogg");
		bounce.Stream = GD.Load<AudioStream>("res://assets/audio/impactPlate_heavy_000.ogg");
	}

	private void InstanceCheck()
	{
		if (Instance is not null && Instance != this)
		{
			QueueFree();
			return;
		}

		if (Instance is null) Instance = this;
	}

	public void PlaySfx(AudioStreamPlayer2D sfx)
	{
		sfx.Play();
	}

	public void StopSfx(AudioStreamPlayer2D sfx)
	{
		sfx.Stop();
	}
}
