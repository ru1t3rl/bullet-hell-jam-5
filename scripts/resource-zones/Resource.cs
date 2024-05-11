using Godot;

namespace BulletHellJam5.ResourceZones.Resources;

public partial class Resource : Node
{
    [Export]
    public string Name { get; set; }

    public Sprite2D Icon { get; set; }

}