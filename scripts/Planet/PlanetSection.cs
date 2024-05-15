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
	}
	
	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is BaseProjectile)
		{
			GD.Print("Colliding with: ", body.Name);
		}
	}

}

