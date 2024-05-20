using Godot;

namespace BulletHellJam5.projectiles;

public partial class BasicProjectile : BaseProjectile
{
    private Sprite2D sprite;
    private CollisionShape2D CollisionShape;

    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        sprite = GetNode<Sprite2D>("ProjectileSprite");
        CollisionShape = GetNode<CollisionShape2D>("ProjectileCollisionShape");
    }

    protected override void Move(double delta)
    {
        GlobalPosition += Velocity * (float)delta;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            this.SetCollisionLayerValue(1, false);
            this.SetCollisionMaskValue(1, false);
            //^ Sets 1st layer to false (Cannot collide with player again)
            //v Sets 2nd layer to true (Can collide with enemy)
            this.SetCollisionLayerValue(2, true);
            this.SetCollisionMaskValue(2, true);

            if (body.HasMethod("GetNormal"))
            {
                Vector2 Normal = (Vector2)body.Call("GetNormal");
                Vector2 Incident = Velocity.Normalized();
                Vector2 Reflected = Incident - 2 * (Incident.Dot(Normal)) * Normal;
                Velocity = Reflected * Velocity.Length(); // Calculate reflected velocity once
            }
            else
            {
                GD.Print("The body does not have the method 'GetNormal'");
            }
        }
    }
}