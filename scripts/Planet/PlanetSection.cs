using System;
using BulletHellJam5.projectiles;
using Godot;

namespace BulletHellJam5.scripts.Planet;

public partial class PlanetSection : Node2D
{

	[Export]
	private int _sectionHealth = 4;

	public override void _Process(double delta)
	{
		if (_sectionHealth <= 0)
		{
			QueueFree();
		}
	}



	private void _on_area_2d_on_collision(Node2D body)
	{
		GD.Print("Colliding with: ", body.Name);
			_sectionHealth--;
			if (body is BasicProjectile)
			{
				GD.Print("Colliding with: ", body.Name);
			}
	}

	

}



