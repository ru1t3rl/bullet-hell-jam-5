using Godot;

public partial class GameManager : Node
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get => _instance;
        set => _instance = value;
    }

    [Export]
    public int Score { get; set; }

    public override void _Ready()
    {
        if (Instance is not null && Instance != this)
        {
            QueueFree();
            return;
        }
        else if (Instance is null)
        {
            Instance = this;
        }
    }
}