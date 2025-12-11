using Godot;
using System;

namespace SpaceShooterPrototype.Autoload
{
    public partial class DirectoryManager : Node
    {
        public static readonly string s_UserDataDirPath = OS.GetUserDataDir();
        public static string s_GameDataPath {get; private set;}
        public static string s_TileVariationsPath {get; private set;}
        public static string s_LevelsPath {get; private set;}

        public override void _Ready()
        {
            if(OS.HasFeature("standalone")) s_GameDataPath = OS.GetExecutablePath() + "/game_data/";
            else s_GameDataPath = s_UserDataDirPath + "/game_data/";

            s_TileVariationsPath = s_GameDataPath + "tile_variations/";
            s_LevelsPath = s_GameDataPath + "levels/";
        }

    }

}
