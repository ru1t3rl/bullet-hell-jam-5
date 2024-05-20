using Godot;

public partial class Player : CharacterBody2D
{
	[Export]
	private float _distanceFromCenter = 5f;

	[Export]
	private float _speed = 100f;

	private Vector2 _input = Vector2.Zero;
	private Vector2 _viewportSize = Vector2.Zero;

	public override void _Ready()
	{
		Vector2 direction = new Vector2(
			Mathf.Cos(Rotation),
			Mathf.Sin(Rotation)
		);

		GlobalPosition = _distanceFromCenter * direction + _viewportSize / 2;
	}

	public override void _Process(double delta)
	{
		_viewportSize = GetViewportRect().Size;
		_input = GetInput();

		if (_input is { X: 0, Y: 0 })
		{
			return;
		}

		Rotation += _speed * _input.X * (float)delta;


		GlobalPosition = _distanceFromCenter * Normal + _viewportSize / 2;
	}

	public Vector2 Normal => new(
		Mathf.Cos(Rotation),
		Mathf.Sin(Rotation)
	);

	private Vector2 GetInput()
	{
		Vector2 input = Vector2.Zero;

		if (Input.IsActionPressed("Rotate_Clockwise"))
		{
			input.X += 1;
		}

		if (Input.IsActionPressed("Rotate_CounterClockwise"))
		{
			input.X -= 1;
		}

		return input;
	}
}
