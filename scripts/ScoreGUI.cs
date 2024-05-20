using Godot;

public partial class ScoreGUI : Label
{
    private void OnScoreChanged(int score)
    {
        Text = score.ToString("0000");
    }
}