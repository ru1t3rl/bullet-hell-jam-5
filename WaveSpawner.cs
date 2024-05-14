using Godot;
using System;
using BulletHellJam5.Enemies;

public partial class WaveSpawner : Node2D
{

	[Export]
	private PackedScene enemy;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetChild<Timer>(0).Start();
	}

	private void _on_wave_spawner_timer_timeout()
	{
		GD.Print("Tick Tock!");
			var instance = enemy.Instantiate();
			if (instance is not BaseEnemy spawnedEnemy)
			{
				GD.Print("Couldn't Find SHIT");
				return;
			}
			AddChild(instance);
			spawnedEnemy.Position = new Vector2(50, 50);
	}
}



