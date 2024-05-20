using System;
using Godot;

public partial class ScoreAdjuster : Node
{
	public enum Method
	{
		Add,
		Subtract
	}

	[Export]
	private Method _action = Method.Add;

	public void AdjustScore(int amount)
	{
		GameManager.Instance.Score += _action switch
		{
			Method.Add => amount,
			Method.Subtract => -amount,
			_ => throw new NotImplementedException()
		};
	}

	public void AdjustScore(int amount, Method action)
	{
		GameManager.Instance.Score += action switch
		{
			Method.Add => amount,
			Method.Subtract => -amount,
			_ => throw new NotImplementedException()
		};
	}
}
