using Godot;
using System;

namespace SpaceShooterPrototype.Level
{
	public partial class TileGridChunk : Node3D
	{
		public void Create(TileGridChunkCreationArgs chunkCreationArgs)
		{
			GlobalPosition = chunkCreationArgs.GlobalPosition;
			OnCreate(chunkCreationArgs);
			/*MeshInstance3D meshInstance = new MeshInstance3D();
			AddChild(meshInstance);
			meshInstance.Mesh = new BoxMesh();
			StandardMaterial3D mat = new StandardMaterial3D();
			mat.AlbedoColor = new Color(chunkCreationArgs.Color);
			mat.AlbedoColor = new Color(mat.AlbedoColor.R, mat.AlbedoColor.G, mat.AlbedoColor.B, 0.75f);
			mat.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
			meshInstance.SetSurfaceOverrideMaterial(0, mat);*/
		}

		protected virtual void OnCreate(TileGridChunkCreationArgs chunkCreationArgs)
		{
			
		}
	}

}
