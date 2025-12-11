using Godot;
using System;
using System.Collections.Generic;

namespace SpaceShooterPrototype.Level
{
	public partial class TileGrid : Node3D
	{
		private readonly Dictionary<string, TileGridLayer> _layers = new();

		/// <summary>
		/// Samples tile map image and creates chunks according to the pixels of the image.
		/// </summary>
		/// <param name="tileMapImage"></param>
		public void Create(Image tileMapImage)
		{
			int w = tileMapImage.GetWidth();
			int h = tileMapImage.GetHeight();

			for(int x = 0; x < tileMapImage.GetWidth(); x++)
			{
				for(int y = 0; y < tileMapImage.GetHeight(); y++)
				{
					string pixelColor = SampleImage(x,y,w,h,tileMapImage);
					TileGridLayer layer;
					if(!_layers.TryGetValue(pixelColor, out layer))
					{
						layer = new TileGridLayer();
						_layers[pixelColor] = layer;
						AddChild(layer);
					}

					layer.TryCreateChunk(
						new Vector2I(x,y),
						pixelColor,
						SampleImage(x + 1,y,w,h,tileMapImage),
						SampleImage(x - 1,y,w,h,tileMapImage),
						SampleImage(x,y + 1,w,h,tileMapImage),
						SampleImage(x,y - 1,w,h,tileMapImage)
					);
						
				}
			}
		}

		private string SampleImage(int x, int y, int w, int h, Image image)
		{
			if(x > 0 && x < w && y > 0 && y < h) return image.GetPixel(x,y).ToHtml(false);
			return "000000";
		}

	}
}
