using BulletHellJam5.Projectiles;
using Godot;

namespace BulletHellJam5.projectiles;

public partial class BasicProjectile : BaseProjectile
{
    bool Reflected = false;

    protected override void Move(double delta)
    {
        if (Reflected == false)
        {
            GlobalPosition += velocity * (float)delta;
        }
        else
        {
            GlobalPosition += -velocity * (float)delta;
        }
    }

    private void _on_area_entered(Area2D area)
    {
        if (area.IsInGroup("Player"))
        {
            GD.Print("Player");
            Reflected = true;
        }
    }
}