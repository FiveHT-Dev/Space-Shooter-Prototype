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
}
