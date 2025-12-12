using Godot;
using SpaceShooterPrototype.Level.MarchingCubes;
using System;
using System.Collections.Generic;

namespace SpaceShooterPrototype.Level
{
	public partial class TileGridMCChunk : TileGridChunk
	{

		private MeshInstance3D _chunkMesh;

		public static int CHUNK_SIZE = 8;
		private float _voxelHalfSize;
		private float _surfaceThreshold = 0.2f;

		private bool _isEmpty = true;
		private bool _dataCreated = false;
		private bool _meshCreated = false;
		private Surface _surface;

		private StandardMaterial3D _debugMaterial;

		private List<MCVoxel> _voxels = new();
		private Dictionary<MCPlane, MCVoxel> _edgeVoxels = new();

		private MCValueOperationStack _operationStack;
		private FastNoiseLite _debugNoise;

		protected override void OnCreate(TileGridChunkCreationArgs chunkCreationArgs)
		{
			if(chunkCreationArgs.Color != "000000")
			{
				_debugNoise = new FastNoiseLite();
				_debugNoise.Frequency = 0.1f;
				_voxelHalfSize = 1.0f / ((float)CHUNK_SIZE * 2.0f);
				_debugMaterial = new StandardMaterial3D();
				_chunkMesh = new MeshInstance3D();
				AddChild(_chunkMesh);
				_surface = new Surface();
				CreateOperationStack(chunkCreationArgs);
				CreateData();
				CreateMesh();
			}
			
		}

		private void CreateOperationStack(TileGridChunkCreationArgs chunkCreationArgs)
		{
			_operationStack = new MCValueOperationStack(chunkCreationArgs);
			_operationStack.AddOperation(new MCVOFoundation(chunkCreationArgs));
			//_operationStack.AddOperation(new MCVONoise(chunkCreationArgs, MCBlendType.Add, _debugNoise));
		}

		public void CreateData()
		{
			// Create voxel grid and sample values at corner points
			for(int x = 0; x < CHUNK_SIZE; x++)
			{
				for(int y = 0; y < CHUNK_SIZE; y++)
				{
					for(int z = 0; z < CHUNK_SIZE; z++)
					{
						Vector3 voxelGridPosition = new Vector3((float)x, (float)y, (float)z);
						MCVoxel voxel = new MCVoxel(_voxelHalfSize, _surfaceThreshold, voxelGridPosition, _operationStack);
						_voxels.Add(voxel);

						if(voxel.CubeIndex != 0 && voxel.CubeIndex != 255)
						{
							_isEmpty = false;
							MarchCube(voxel, _surface);
						}
					}
				}
			}

			_dataCreated = true;
		}

		public void CreateMesh()
		{
			if(_meshCreated) return;
			
			if(_surface.Verts.Count != 0)
			{
				_chunkMesh.Mesh = _surface.GetAsArrayMesh();
				_chunkMesh.CreateTrimeshCollision();
				_chunkMesh.SetSurfaceOverrideMaterial(0, _debugMaterial);
			} 

			_meshCreated = true;
		}

		private void MarchCube(MCVoxel voxel, Surface surface)
		{
			if(voxel.CubeIndex == 0 || voxel.CubeIndex == 255) return;

			int edgeIndex = 0;

			for(int i = 0; i < 5; i++)
			{
				List<Vector3> tri = [];
				for(int j = 0; j < 3; j++)
				{
					int index = MCTables.s_TriangleTable[voxel.CubeIndex][edgeIndex];

					if(index == -1) return;

					Vector3 edgeVert1 = voxel.Points[MCTables.s_EdgeVertexIndices[index].Vi0].Position;
					Vector3 edgeVert2 = voxel.Points[MCTables.s_EdgeVertexIndices[index].Vi1].Position;

					//Vector3 vertex = (edgeVert1 + edgeVert2) / 2.0f;

					Vector3 vertex = edgeVert1.Lerp(edgeVert2, 0.5f);

					tri.Add(vertex);
					
					  
					edgeIndex++;
				}

				Vector3 normal = MarchingCubesUtils.CalculateTriangleNormal(tri);

				foreach(Vector3 v in tri)
				{
					surface.Verts.Add(v);
					surface.Indices.Add(surface.Verts.Count - 1);
					surface.UVs.Add(Vector2.Zero);
					surface.Normals.Add(normal);
				}
			}
		}
	}
}
