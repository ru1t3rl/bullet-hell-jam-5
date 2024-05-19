using Godot;
using static Godot.GD;
using static Godot.Mathf;
using System.Reflection;

namespace BulletHellJam5.projectiles;

public partial class BasicProjectile : BaseProjectile
{
	bool isReflected = false;
	Vector2 Normal;
	Vector2 reflectedVelocity;
	Sprite2D sprite;
	CollisionShape2D CollisionShape;
	
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		sprite = GetNode<Sprite2D>("ProjectileSprite");
		CollisionShape = GetNode<CollisionShape2D>("ProjectileCollisionShape");
	}
	
	protected override void Move(double delta)
	{
		Vector2 currentVelocity = isReflected ? reflectedVelocity : velocity;
		GlobalPosition += currentVelocity * (float)delta;
		//if (isReflected){UpdateSpriteRotation(currentVelocity);}
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
				Normal = (Vector2)body.Call("GetNormal");
				Vector2 Incident = velocity.Normalized();
				Vector2 Reflected = Incident - 2 * (Incident.Dot(Normal)) * Normal;
				reflectedVelocity = Reflected * velocity.Length(); // Calculate reflected velocity once
				isReflected = true;
			}
				else
				{
					GD.Print("The body does not have the method 'GetNormal'");
				}
		}
	}
	//private void UpdateSpriteRotation(Vector2 currentVelocity)
	//{
	//	if (sprite != null)
	//	{
	//		// Get the angle of the velocity vector in radians
	//		float angle = Mathf.RadToDeg(currentVelocity.Angle());
	//		// Update the sprite's rotation (Godot uses radians for rotation)
	//		sprite.Rotation = angle;
	//		CollisionShape.Rotation = angle;
	//	}
	//}
}

