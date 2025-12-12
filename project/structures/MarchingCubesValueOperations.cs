using Godot;
using System;
using System.Collections.Generic;

namespace SpaceShooterPrototype.Level.MarchingCubes
{
    public class MCValueOperationStack
    {
        private List<MCValueOperation> _stack = new();
        private TileGridChunkCreationArgs _chunkCreationArgs;
        public MCValueOperationStack(TileGridChunkCreationArgs chunkCreationArgs)
        {
            _chunkCreationArgs = chunkCreationArgs;
        }

        public void AddOperation(MCValueOperation operation)
        {
            _stack.Add(operation);
        }

        public float GetValue(MCCornerPoint point)
        {
            float value = 0.0f;
            foreach(MCValueOperation o in _stack)
            {
                value = o.Execute(value, point);
            }

            return value;
        }
    }

    public class MCValueOperation
    {
        private MCBlendType _blendType;
        protected TileGridChunkCreationArgs _chunkCreationArgs;
        public MCValueOperation(TileGridChunkCreationArgs chunkCreationArgs, MCBlendType blendType)
        {
            _chunkCreationArgs = chunkCreationArgs;
            _blendType = blendType;
        }
        public float Execute(float previous, MCCornerPoint point)
        {
            float value = OnCalculateValue(point);
            float newValue = 0.0f;
            switch (_blendType)
            {
                case MCBlendType.Multiply: newValue = previous * value; break;
                case MCBlendType.Subtract: newValue = previous - value; break;
                case MCBlendType.Add: newValue = previous + value; break;
                case MCBlendType.Divide: if(previous > 0.0f) newValue = previous / value; else newValue = previous; break;
            }

            return Mathf.Clamp(newValue, 0.0f, 1.0f);
        }

        protected virtual float OnCalculateValue(MCCornerPoint point)
        {
            return 0.0f;
        }
    }

    public class MCVOFoundation : MCValueOperation
    {
        public MCVOFoundation(TileGridChunkCreationArgs chunkCreationArgs) : base(chunkCreationArgs, MCBlendType.Add){}

        protected override float OnCalculateValue(MCCornerPoint point)
        {
            int w = _chunkCreationArgs.ScaledTileMapImage.GetWidth();
            int h = _chunkCreationArgs.ScaledTileMapImage.GetHeight();
            int x = (int)((point.Position.X + _chunkCreationArgs.GlobalPosition.X) * (float)TileGridMCChunk.CHUNK_SIZE);
            int y = (int)((point.Position.Y + _chunkCreationArgs.GlobalPosition.Y) * (float)TileGridMCChunk.CHUNK_SIZE); 
            int z = (int)(point.Position.Z * (float)TileGridMCChunk.CHUNK_SIZE);

            

            /*string pointColor = TileGrid.SampleImage(x,y,w,h,_chunkCreationArgs.ScaledTileMapImage);

            string nPosXColor = TileGrid.SampleImage(x + 1,y,w,h,_chunkCreationArgs.ScaledTileMapImage);
            string nNegXColor = TileGrid.SampleImage(x - 1,y,w,h,_chunkCreationArgs.ScaledTileMapImage);

            string nPosYColor = TileGrid.SampleImage(x,y + 1,w,h,_chunkCreationArgs.ScaledTileMapImage);
            string nNegYColor = TileGrid.SampleImage(x,y - 1,w,h,_chunkCreationArgs.ScaledTileMapImage);*/

            string color = TileGrid.SampleImage(x,y,w,h,_chunkCreationArgs.ScaledTileMapImage);
            

            bool[] neighbours = 
            [
                TileGrid.SampleImage(x + 1,y,w,h,_chunkCreationArgs.ScaledTileMapImage) == _chunkCreationArgs.Color,
                TileGrid.SampleImage(x - 1,y,w,h,_chunkCreationArgs.ScaledTileMapImage) == _chunkCreationArgs.Color,
                TileGrid.SampleImage(x,y + 1,w,h,_chunkCreationArgs.ScaledTileMapImage) == _chunkCreationArgs.Color,
                TileGrid.SampleImage(x,y - 1,w,h,_chunkCreationArgs.ScaledTileMapImage) == _chunkCreationArgs.Color,
                z < TileGridMCChunk.CHUNK_SIZE - 1,
                z > 0
            ];

            foreach(bool n in neighbours) if(!n) return 0.0f;
            return 1.0f;
        }
    }

    public class MCVONoise : MCValueOperation
    {
        private FastNoiseLite _noise;
        public MCVONoise(TileGridChunkCreationArgs chunkCreationArgs, MCBlendType blendType, FastNoiseLite noise) : base(chunkCreationArgs, blendType)
        {
            _noise = noise;   
        }

        protected override float OnCalculateValue(MCCornerPoint point)
        {
            Vector3 pos = new Vector3(point.Position.X + _chunkCreationArgs.GlobalPosition.X, point.Position.Y + _chunkCreationArgs.GlobalPosition.Y, point.Position.Z);
            return _noise.GetNoise3Dv(point.Position);
        }
    }

    public enum MCBlendType
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}