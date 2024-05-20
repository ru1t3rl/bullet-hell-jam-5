using System;
using System.Collections.Generic;
using BulletHellJam5.Enemies;
using Godot;

public partial class WaveManager : Node2D
{
    // Define variables to store wave data
    private int currentWaveIndex = 0;
    // Remaining spawn score for the current wave
    private int spawnScore;
    // Example spawn scores for each wave
    [Export]
    private int[] waveSpawnScores = { 150, 150, 150 };
    // Load enemy scene here
    [Export]
    private PackedScene[] enemyScenes;
    // Lists of enemy scenes for each wave
    private List<PackedScene>[] waveEnemyLists;
    private Timer spawnTimer;

    private int _seed; // seed for random number Gen
    private Random random;

    [Export]
    private int _minDistanceOutsideViewport = 50;
    [Export]
    private int _maxDistanceOutsideViewport = 100;


    [Signal]
    public delegate void OnWaveStartEventHandler(int wave, int nWaves);

    [Export]
    public int Seed // Property to set the seed value from the editor
    {
        get => _seed;
        set => _seed = value;
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

            EmitSignal(nameof(OnWaveStart), currentWaveIndex + 1, waveSpawnScores.Length);
        }
    }


    private Vector2 GetRandomPositionOutsideViewport()
    {
        Vector2 viewportSize = GetViewportRect().Size;
        // 0: left, 1: top, 2: right, 3: bottom
        int side = random.Next(4);
        return side switch
        {
            0 => new Vector2(
                -random.Next(_minDistanceOutsideViewport, _maxDistanceOutsideViewport),
                random.NextSingle() * viewportSize.Y),
            1 => new Vector2(
                random.NextSingle() * viewportSize.X,
                -random.Next(_minDistanceOutsideViewport, _maxDistanceOutsideViewport)),
            2 => new Vector2(
                random.Next(_minDistanceOutsideViewport, _maxDistanceOutsideViewport),
                random.NextSingle() * viewportSize.Y),
            3 => new Vector2(
                random.NextSingle() * viewportSize.X,
                random.Next(_minDistanceOutsideViewport, _maxDistanceOutsideViewport)),
            _ => throw new ArgumentException()
        };
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
                int randomIndex = random.Next(0, waveEnemyLists[currentWaveIndex].Count - 1);
                var enemyScene = waveEnemyLists[currentWaveIndex][randomIndex];

                // Instantiate the selected enemy scene
                var instance = enemyScene.Instantiate();
                // Check if instantiated node is an enemy
                if (instance is not BaseEnemy spawnedEnemy)
                {
                    return;
                }

                // Check if the spawn score is sufficient to spawn an enemy
                if (spawnScore >= spawnedEnemy.Score)
                {
                    // Add enemy to the scene and set its position and deduct from score value
                    AddChild(instance);
                    spawnedEnemy.Position = GetRandomPositionOutsideViewport();
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