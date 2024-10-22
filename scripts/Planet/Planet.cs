using Godot;
using System;
using System.Collections.Generic;
using BulletHellJam5.ResourceZones;
using BulletHellJam5.scripts.Planet;

public partial class Planet : Node2D
{
	private Vector2 _viewPortSize;
	[Export] private int _maxZones = 3; 
	private int _destroyedZones = 0;
	[Export] private PackedScene[] _resourceZones;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Setup();
	}

	[Export]
	private float _radius = 0;

	private void Setup()
	{
		_viewPortSize = GetViewportRect().Size;
		Position = _viewPortSize / 2;

		float angleStep = Mathf.Tau / _maxZones; // Calculate the angle between each zone

		for (int i = 0; i < _maxZones; i++)
		{
			// Calculate the position of the zone using polar coordinates
			float angle = i * angleStep;
			float x = Position.X + _radius * Mathf.Cos(angle);
			float y = Position.Y + _radius * Mathf.Sin(angle);

			
			var instance = _resourceZones[0].Instantiate();
			if (instance is not BaseResourceZone resourceZone)
			{
				//GD.Print("Couldn't find zones!");
				return;
			}
			
			AddChild(instance);
			resourceZone.GlobalPosition = new Vector2(x, y);
			resourceZone.GlobalRotation = angle;
			resourceZone.OnZoneDestroyed += OnZoneDestroyed;
		}
	}
	private void OnZoneDestroyed(int amount)
	{
		_destroyedZones += amount;
		GD.Print($"Zone destroyed: {_destroyedZones}/{_maxZones}");

		if (_destroyedZones >= _maxZones / 2)
		{
			EndGame();
		}
	}

	  private void EndGame()
	{
		GD.Print("Game Over: Planet destroyed!");
		AudioManager.Instance.PlaySfx(AudioManager.Instance.planetSfx);
		CallDeferred(nameof(RestartGame));
	}
	
	private void RestartGame()
	{
		GetTree().ReloadCurrentScene();
	}
}
