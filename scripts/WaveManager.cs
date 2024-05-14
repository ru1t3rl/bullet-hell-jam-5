using Godot;
using System;
using BulletHellJam5.Enemies;

public partial class WaveManager : Node2D
{
	// Define variables to store wave data
	private int currentWaveIndex = 0;
	private int spawnScore;
	[Export] private int[] waveSpawnScores = { 15, 15,15 }; // Example spawn scores for each wave
	[Export] private PackedScene enemyScene; // Load enemy scene here

	private Timer spawnTimer;

	public override void _Ready()
	{
		spawnTimer = GetNode<Timer>("Timer");
		// Start spawning waves
		SpawnWave();
	}

	private void SpawnWave()
	{
		// Check if there are more waves to spawn
		if (currentWaveIndex < waveSpawnScores.Length)
		{
			spawnTimer.Start();
			spawnScore = waveSpawnScores[currentWaveIndex];
		}
	}

	private Vector2 GetRandomSpawnPositionOutsideCameraView()
	{
		// Get the size of the viewport
		Vector2 viewportSize = GetViewportRect().Size;

		// Define the boundaries outside of the viewport
		float minX = -viewportSize.X / 2;
		float maxX = viewportSize.X / 2;
		float minY = -viewportSize.Y / 2;
		float maxY = viewportSize.Y / 2;

		// Generate random coordinates within the boundaries
		float randomX = (float)GD.RandRange(minX, maxX);
		float randomY = (float)GD.RandRange(minY, maxY);

		// Return the random position
		return new Vector2(randomX, randomY);
	}

	private void _on_timer_timeout()
	{
		if (!spawnTimer.IsStopped())
		{
			var instance = enemyScene.Instantiate();
			if (instance is not BaseEnemy spawnedEnemy)
			{
				GD.Print("Couldn't Find SHIT");
				return;
			}

			AddChild(instance);
			spawnedEnemy.Position = GetRandomSpawnPositionOutsideCameraView();
			spawnScore -= spawnedEnemy.Score;
			GD.Print("Wave: ", currentWaveIndex);
			GD.Print("Scores: ", spawnScore);

			// Check if spawn score is depleted or timer should stop
			if (spawnScore <= 0)
			{
				currentWaveIndex++;
				spawnTimer.Stop();
				SpawnWave(); // Move to the next wave
			}
		}
	}
}
