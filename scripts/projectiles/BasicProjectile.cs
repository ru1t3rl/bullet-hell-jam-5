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
	Vector2 initialDirection;
	Sprite2D sprite;
	CollisionShape2D CollisionShape;
	
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	
	protected override void Move(double delta)
	{
		var _rotationSpeed = 5;
		
		Vector2 currentVelocity = isReflected ? reflectedVelocity : velocity;
		GlobalPosition += currentVelocity * (float)delta;
		float targetAngle = float.Atan2(currentVelocity.Y, currentVelocity.X);
		Rotation = float.Lerp(Rotation, targetAngle, (float)delta * _rotationSpeed);
		//this.Rotation = Mathf.RadToDeg(Rotation);
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
}

