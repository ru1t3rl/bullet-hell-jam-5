using Godot;

public partial class GameManager
{
    [Signal]
    public delegate void OnAdjustScoreEventHandler(int score);
}