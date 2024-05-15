using Godot;
using System;

public partial class Planet : Node2D
{
	private Vector2 _viewPortSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Setup();
	}

	private void Setup()
	{
		_viewPortSize = GetViewportRect().Size;
		Position = new Vector2(_viewPortSize.X/2,_viewPortSize.Y/2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
