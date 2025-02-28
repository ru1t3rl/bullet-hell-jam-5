﻿using Godot;

namespace BulletHellJam5.Projectiles;

public partial class BaseProjectile
{
    [Signal]
    public delegate void OnLifespanReachedEventHandler();

    [Signal]
    public delegate void OnCollisionEventHandler(Node2D body);
}