using Godot;

public partial class GameManager : Node
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get => _instance;
        set => _instance = value;
    }

    private int _score;

    [Export]
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            EmitSignal(nameof(OnAdjustScore), _score);
        }
    }

    public override void _Ready()
    {
        if (Instance is not null && Instance != this)
        {
            QueueFree();
            return;
        }

        if (Instance is null)
        {
            Instance = this;
        }
    }

    protected override void Dispose(bool disposing)
    {
        Instance = null;
        base.Dispose(disposing);
    }
}