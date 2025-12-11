using Godot;
using System;

namespace SpaceShooterPrototype.Level
{
	public partial class TileGridChunk : Node3D
	{
		public void Create(Vector2I pos)
		{
			GlobalPosition = new Vector3((float)pos.X, (float)pos.Y, 0.0f);
		}
	}

}
