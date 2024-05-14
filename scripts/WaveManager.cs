using Godot;
using System;
using BulletHellJam5.Enemies;

public partial class WaveManager : Node2D
{
	// Define variables to store wave data
	private int currentWaveIndex = 0;
	
	[Export]
	private int[] waveSpawnScores = { 100, 200, 300 }; // Example spawn scores for each wave
	[Export]
	private PackedScene enemyScene; // Load enemy scene here

	public override void _Ready()
	{
		
		// Start spawning waves
		SpawnWave();
	}

	private void SpawnWave()
	{
		// Check if there are more waves to spawn
		if (currentWaveIndex < waveSpawnScores.Length)
		{
			// Spawn enemies based on spawn score
			int spawnScore = waveSpawnScores[currentWaveIndex];

			while (spawnScore > 0)
			{
				var instance = enemyScene.Instantiate();
				if (instance is not BaseEnemy spawnedEnemy)
				{
					GD.Print("Couldn't Find SHIT");
					return;					
				}
				AddChild(instance);
				spawnedEnemy.Position = GetRandomSpawnPositionOutsideCameraView();
				GD.Print("Wave ", currentWaveIndex);
				spawnScore -= spawnedEnemy.Score;
			}
			currentWaveIndex++; // Move to the next wave
		}
	}

	private Vector2 GetRandomSpawnPositionOutsideCameraView()
	{
		// Logic to generate a random position outside of camera view
		// You can customize this based on your game's design
		// For simplicity, let's assume enemies spawn at fixed positions outside of camera view
		// You may need to adjust this based on your game's requirements
		return new Vector2(-100, -100); // Example position outside of camera view
	}
}
