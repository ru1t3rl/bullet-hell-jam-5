using Godot;

namespace BulletHellJam5.ResourceZones;

public partial class BaseResourceZone
{
    [Signal]
    public delegate void OnTakeDamageEventHandler(int amount);

    [Signal]
    public delegate void OnCollisionEventHandler(Node2D body);

    [Signal]
    public delegate void OnTakeResourcesEventHandler(int amount);

    [Signal]
    public delegate void OnResourcesGeneratedEventHandler(int numberOfResources);
}