using BulletHellJam5.ResourceZones.Resources;
using Godot;

namespace BulletHellJam5.ResourceZones;

public partial class BaseResourceZone : Area2D
{
    [Export]
    private Currency _currency;

    [Export]
    private int _health = 50;

    [ExportGroup("Resource Generation")]
    [Export]
    private int _numberOfResourcesPerGeneration = 10;
    [Export]
    private float _resourceGenerationInterval = 5f;
    private Timer _resourceGenerationTimer = new();

    private int _currentNumberOfResources;

    public override void _Ready()
    {
        if (_currency is null)
        {
            GD.Print("Linked supply resources isn't a valid supply");
            return;
        }

        SetupTimer();
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
    }

    public void TakeResources(int amount)
    {
        _currentNumberOfResources -= amount;
    }

    public int TakeAllResources()
    {
        var nResources = _currentNumberOfResources;
        _currentNumberOfResources = 0;
        return nResources;
    }

    private void SetupTimer()
    {
        AddChild(_resourceGenerationTimer);
        _resourceGenerationTimer.WaitTime = _resourceGenerationInterval;
        _resourceGenerationTimer.Timeout += OnResourceTimerTick;
        _resourceGenerationTimer.Start();
    }

    private void OnResourceTimerTick()
    {
        _currentNumberOfResources += _numberOfResourcesPerGeneration;
    }
}