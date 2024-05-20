using BulletHellJam5.Projectiles;
using Godot;

namespace BulletHellJam5.projectiles;

public partial class BasicProjectile : BaseProjectile
{
    protected override void Move(double delta)
    {
        GlobalPosition += Velocity * (float)delta;
    }

    protected override void OnCollisionWithBody(Node2D body)
    {
        base.OnCollisionWithBody(body);

        if (body is Player player)
        {
            this.SetCollisionLayerValue(1, false);
            this.SetCollisionMaskValue(1, false);
            //^ Sets 1st layer to false (Cannot collide with player again)
            //v Sets 2nd layer to true (Can collide with enemy)
            this.SetCollisionLayerValue(2, true);
            this.SetCollisionMaskValue(2, true);

            Vector2 incident = Velocity.Normalized();
            Vector2 reflected = incident - 2 * (incident.Dot(player.Normal)) * player.Normal;
            Velocity = reflected * Velocity.Length(); // Calculate reflected velocity once
        }
    }
}