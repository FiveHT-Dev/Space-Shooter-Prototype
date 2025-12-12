using Godot;
using System;
using System.IO;
using System.Text.Json;

namespace SpaceShooterPrototype.Level
{
    // NOTE: JsonSerializer.Deserialize only works if the properties of a class are public and have {get; set;}.
    // Things are done like this to keep access modifiers clean.
    public class JsonTileVariation
    {
        public string VariationName {get;set;}
        public string VariationColor {get;set;}
    }

    public class TileVariation
    {
        public string VariationName {get{return _variationName;}}
        public string VariationColor {get{return _variationColor;}}
        private string _variationName = "NONAME";
        private string _variationColor = "NOCOLOR";
        private string _filePath;
        
        
        public TileVariation(string folderPath, string fileName)
        {
            _filePath = folderPath + "/" + fileName; 
            if(_filePath.EndsWith(".json") && File.Exists(_filePath))
            {
                JsonTileVariation loadedJsonFile = JsonSerializer.Deserialize<JsonTileVariation>(File.ReadAllText(_filePath));
                _variationName = loadedJsonFile.VariationName;
                _variationColor = loadedJsonFile.VariationColor;
            }
        }
    }

    public class TileGridChunkCreationArgs
    {
        public Image ScaledTileMapImage {get; private set;}
        public Vector3 GlobalPosition {get; private set;}
        public string Color {get; private set;}
        public string NPosXColor {get; private set;}
        public string NNegXColor {get; private set;}
        public string NPosYColor {get; private set;}
        public string NNegYColor {get; private set;}

        public TileGridChunkCreationArgs(string color, Image scaledTileMapImage, Vector3 globalPosition, string nPosXColor, string nNegXColor, string nPosYColor, string nNegYColor)
        {
            Color = color;
            ScaledTileMapImage = scaledTileMapImage;
            GlobalPosition = globalPosition;
            NPosXColor = nPosXColor;
            NNegXColor = nNegXColor;
            NPosYColor = nPosYColor;
            NNegYColor = nNegYColor;
        }
    }
}
