namespace BulletHellJam5.projectiles;

public partial class BasicProjectile : BaseProjectile
{
    protected override void Move(double delta)
    {
        GlobalPosition += velocity * (float)delta;
    }
}