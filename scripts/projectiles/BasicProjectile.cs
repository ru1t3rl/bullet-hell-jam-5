using Godot;

namespace BulletHellJam5.projectiles;

public partial class BasicProjectile : BaseProjectile
{

	protected override void Move(double delta)
	{
		GlobalPosition += velocity * (float)delta;
	}
	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player"))
		{
			//get_parent()
		}
	}
}




