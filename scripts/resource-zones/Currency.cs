using System;
using Godot;

namespace BulletHellJam5.ResourceZones.Resources;

[Serializable]
public partial class Currency : Resource
{
	[Export]
	public string Name { get; set; }

	[Export]
	public Texture Icon { get; set; }
}
