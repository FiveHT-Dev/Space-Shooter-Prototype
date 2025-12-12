using Godot;
using SpaceShooterPrototype.Autoload;
using System;
using System.IO;
using System.Text.Json;

namespace SpaceShooterPrototype.Level
{
	public class JsonLevel
	{
		public string LevelName {get;set;}
	}
	public partial class Level : Node3D
	{
		[Export] private string _lvlFolderPathFromEditor;
		[Export] private string _lvlFileNameFromEditor;
		private bool _created = false;
		private bool _dataLoaded = false;
		private string _levelName;
		private Image _tileMapImage;
		private Image _scaledTileMapImage;
		private TileGrid _tileGrid;

		public override void _Ready()
		{
			if(_lvlFolderPathFromEditor != "" && _lvlFileNameFromEditor != "") 
				Create(DirectoryManager.s_LevelsPath + _lvlFolderPathFromEditor, _lvlFileNameFromEditor);
		}

		private bool LoadData(string lvlFolderPath, string lvlFileName)
		{
			bool fileLoaded = false;
			bool tileMapLoaded = false;
			foreach(string file in Directory.GetFiles(lvlFolderPath))
			{
				string sanitizedFile = file.Replace(@"\", "/");
				string[] slices = sanitizedFile.Split("/");
                string fileName = slices[slices.Length - 1];
				if(fileName.EndsWith(".json") && fileName.StartsWith("lvl"))
                {
                    JsonLevel jsonLevel = JsonSerializer.Deserialize<JsonLevel>(File.ReadAllText(sanitizedFile));
                    _levelName = jsonLevel.LevelName;
                    fileLoaded = true;
                }
				else if(fileName.EndsWith("bmp") && fileName.StartsWith("tlmp"))
                {
                    _tileMapImage = Image.LoadFromFile(sanitizedFile);
					_scaledTileMapImage = Image.LoadFromFile(sanitizedFile);
					_scaledTileMapImage.Resize(
						_scaledTileMapImage.GetWidth() * TileGridMCChunk.CHUNK_SIZE,
						_scaledTileMapImage.GetHeight() * TileGridMCChunk.CHUNK_SIZE,
						Image.Interpolation.Nearest
						);
                    tileMapLoaded = true;
                }
            }

            return fileLoaded && tileMapLoaded;
            
        }

		public void Create(string lvlFolderPath, string lvlFileName)
		{
			if (!_created)
			{
                _dataLoaded = LoadData(lvlFolderPath, lvlFileName);
				if(_dataLoaded)
                {
                    _tileGrid = new TileGrid();
                    AddChild(_tileGrid);
                    _tileGrid.Create(_tileMapImage, _scaledTileMapImage);
                    _created = true;
                }
                
            }
        }

    }

}
