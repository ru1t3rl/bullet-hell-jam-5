using BulletHellJam5.Enemies.Enums;
using Godot;

namespace BulletHellJam5.Enemies;

public partial class BaseEnemy
{
	[Signal]
	public delegate void OnShootEventHandler();

	[Signal]
	public delegate void OnTakeDamageEventHandler(float health);

	[Signal]
	public delegate void OnStateChangeEventHandler(EnemyState state);

	[Signal]
	public delegate void OnDieEventHandler(BaseEnemy enemy);
}
