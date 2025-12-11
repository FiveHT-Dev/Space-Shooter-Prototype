using Godot;
using SpaceShooterPrototype.Level;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceShooterPrototype.Autoload
{
    public partial class Resources : Node
    {
        public static readonly Dictionary<string, TileVariation> s_TileVariationsByName = new();
        public static readonly Dictionary<string, TileVariation> s_TileVariationsByColor = new();
        public override void _Ready()
        {
            LoadTileVariations();
        }

        private static void LoadTileVariations()
        {
            foreach(string variationFolderPath in Directory.GetDirectories(DirectoryManager.s_TileVariationsPath))
            {
                foreach(string variationFilePath in Directory.GetFiles(variationFolderPath))
                {
                    string sanitizedVariationFilePath = variationFilePath.Replace(@"\", "/");
                    string[] slices = sanitizedVariationFilePath.Split(@"/");
                    string variationFileName = slices[slices.Length - 1];
                    if(variationFileName.StartsWith("tv") && variationFileName.EndsWith(".json"))
                    {
                        TileVariation tileVariation = new TileVariation(variationFolderPath, variationFileName);
                        if(tileVariation.VariationName != "NONAME") s_TileVariationsByName[tileVariation.VariationName] = tileVariation;
                        if(tileVariation.VariationColor != "NOCOLOR") s_TileVariationsByColor[tileVariation.VariationColor] = tileVariation;
                    }
                }
            }
        }


    }

}
