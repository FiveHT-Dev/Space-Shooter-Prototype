using Godot;
using System;
using System.Collections.Generic;

namespace SpaceShooterPrototype.Level
{
	public partial class TileGridLayer : Node3D
	{
		private Dictionary<Vector2I, TileGridChunk> _chunks = new();

		public void TryCreateChunk(TileGridChunkCreationArgs chunkCreationArgs)
		{
			if(chunkCreationArgs.Color != "000000")
			{
				TileGridChunk chunk = new TileGridMCChunk();
				AddChild(chunk);
				chunk.Create(chunkCreationArgs);
			}
		}
		
	}

}
