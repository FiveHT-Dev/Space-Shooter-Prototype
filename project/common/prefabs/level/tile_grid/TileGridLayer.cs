using Godot;
using System;
using System.Collections.Generic;

namespace SpaceShooterPrototype.Level
{
	public partial class TileGridLayer : Node3D
	{
		private Dictionary<Vector2I, TileGridChunk> _chunks = new();

		public void TryCreateChunk(Vector2I pos, string chunkColor, string nPosXColor, string nNegXColor, string nPosYColor, string nNegYColor)
		{
			if(chunkColor != "000000")
			{
				TileGridChunk chunk = new TileGridChunk();
				AddChild(chunk);
				chunk.Create(pos);

				MeshInstance3D meshInstance = new MeshInstance3D();
				chunk.AddChild(meshInstance);
				meshInstance.Mesh = new BoxMesh();
				StandardMaterial3D mat = new StandardMaterial3D();
				mat.AlbedoColor = new Color(chunkColor);
				meshInstance.SetSurfaceOverrideMaterial(0, mat);
			}
		}
		
	}

}
