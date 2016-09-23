using System;
using System.IO;
using System.Collections.Generic;

namespace Zaphat.Assets.Textures
{
	public class TextureManager : IAssetManager<Texture>
	{
		Dictionary<Guid, Texture> index = new Dictionary<Guid, Texture>();
		Dictionary<string, Texture> pathIndex = new Dictionary<string, Texture>();

		public Texture Load(Stream stream)
		{

		}

		public Texture Load(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Load(fs);
			}
		}

		public void Unload(Texture asset)
		{
			var id = asset.ID;
			var path = asset.Path;
			asset.Release();
			index.Remove(id);
			pathIndex.Remove(path);
		}
	}
}
