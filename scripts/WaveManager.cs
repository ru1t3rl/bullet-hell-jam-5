using System;
using Godot;
using System.Collections.Generic;
using BulletHellJam5.Enemies;
using static System.Random;

public partial class WaveManager : Node2D
{
	// Define variables to store wave data
	private int currentWaveIndex = 0; 
	// Remaining spawn score for the current wave
	private int spawnScore; 
	// Example spawn scores for each wave
	[Export] private int[] waveSpawnScores = { 150, 150, 150 }; 
	// Load enemy scene here
	[Export] private PackedScene[] enemyScenes; 
	// Lists of enemy scenes for each wave
	private List<PackedScene>[] waveEnemyLists; 
	private Timer spawnTimer;

	private int _seed; // seed for random number Gen
	private Random random;

	[Export]
	public int Seed // Property to set the seed value from the editor
	{
		get { return _seed; }
		set { _seed = value; }
	}
	
	public override void _Ready()
	{
		spawnTimer = GetNode<Timer>("Timer");

		random = new Random(_seed);
		
		// Initialize the list of enemies for each wave
		InitializeWaveEnemyLists();
		
		// Start spawning waves
		SpawnWave();
	}

	// Initialize the list of enemies for each wave
	private void InitializeWaveEnemyLists()
	{

		// Initialize the array to hold lists of enemy scenes per wave
		waveEnemyLists = new List<PackedScene>[waveSpawnScores.Length];
		for (int i = 0; i < waveSpawnScores.Length; i++)
		{
			// Create a new list for each wave and add all enemy scenes for current wave
			waveEnemyLists[i] = new List<PackedScene>();
			waveEnemyLists[i].AddRange(enemyScenes);
		}
	}

	private void SpawnWave()
	{
		// Check if there are more waves to spawn and start the timer
		if (currentWaveIndex < waveSpawnScores.Length)
		{
			spawnTimer.Start();
			spawnScore = waveSpawnScores[currentWaveIndex];
		}
	}


	// Generate a random spawnPosition around the edge of the camera view
	private Vector2 GetRandomSpawnPositionOutsideCameraView()
	{
		// Get the size of the viewport
		Vector2 viewportSize = GetViewportRect().Size;

		// Define the spawn boundaries outside of the viewport
		float minX = -viewportSize.X / 2, maxX = viewportSize.X / 2;
		float minY = -viewportSize.Y / 2, maxY = viewportSize.Y / 2;

		// Generate random coordinates within the boundaries
		float randomX = (float)GD.RandRange(minX, maxX);
		float randomY = (float)GD.RandRange(minY, maxY);

		// Return the random position
		return new Vector2(randomX, randomY);
	}

	//Handle Timer timeout event which spawns enemies
	private void _on_timer_timeout()
	{
		// Check if the timer is running and currentWaveIndex is within bounds
		if (!spawnTimer.IsStopped() && currentWaveIndex < waveEnemyLists.Length)
		{
			// Check if there are enemies left to spawn
			if (spawnScore > 0 && waveEnemyLists[currentWaveIndex].Count > 0)
			{
				// Select a random enemy scene from one the list
				int randomIndex = random.Next(0, waveEnemyLists[currentWaveIndex].Count-1);
				var enemyScene = waveEnemyLists[currentWaveIndex][randomIndex];

				// Instantiate the selected enemy scene
				var instance = enemyScene.Instantiate();
				// Check if instantiated node is an enemy
				if (instance is not BaseEnemy spawnedEnemy)
					return;
				
				// Check if the spawn score is sufficient to spawn an enemy
				if (spawnScore >= spawnedEnemy.Score)
				{
					// Add enemy to the scene and set its position and deduct from score value
					AddChild(instance);
					spawnedEnemy.Position = GetRandomSpawnPositionOutsideCameraView();
					spawnScore -= spawnedEnemy.Score;
				}

				// Check if spawn score is depleted or timer should stop
				if (spawnScore <= 0)
				{
					// Increase wave index, stop the timer and start spawning the next wave
					currentWaveIndex++;
					spawnTimer.Stop();
					SpawnWave(); 
				}
			}
		}
		else
		{
			// If currentWaveIndex is out of bounds, stop the timer
			spawnTimer.Stop();
		}
	}
}