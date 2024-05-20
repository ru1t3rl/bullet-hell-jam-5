using Godot;

public partial class ScreenShake : Node
{
    [Export]
    private float _maxIntensity;

    [Export]
    private double _shakeDuration;

    [Export]
    private Node2D _objectToShake;
    private Vector2 _startPosition;

    private double _shakeTime;

    private static ScreenShake _instance;

    public static ScreenShake Instance
    {
        get => _instance;
        set => _instance = value;
    }

    public override void _Ready()
    {
        if (Instance is not null && Instance != this)
        {
            QueueFree();
        }
        else if (Instance is null)
        {
            Instance = this;
        }

        _objectToShake ??= GetParent<Node2D>();
        _shakeTime = _shakeDuration;
    }

    public override void _Process(double delta)
    {
        if (_shakeTime >= _shakeDuration)
        {
            _objectToShake.Position = _startPosition;
            return;
        }

        _objectToShake.Position += new Vector2(
            (float)GD.RandRange(-_maxIntensity, _maxIntensity),
            (float)GD.RandRange(-_maxIntensity, _maxIntensity)
        );

        _shakeTime += delta;
    }

    public void Shake()
    {
        _startPosition = new Vector2(_objectToShake.Position.X, _objectToShake.Position.Y);
        _shakeTime = 0;
    }

    protected override void Dispose(bool disposing)
    {
        _objectToShake = null;
        Instance = null;
        base.Dispose(disposing);
    }
}