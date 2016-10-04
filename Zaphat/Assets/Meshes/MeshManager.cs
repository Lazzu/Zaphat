using System;
using System.Collections.Generic;
using System.IO;
using Zaphat.Utilities;

namespace Zaphat.Assets.Meshes
{
	public class MeshManager : IAssetManager<Mesh>
	{
		Dictionary<Guid, Mesh> index = new Dictionary<Guid, Mesh>();
		Dictionary<string, Mesh> pathIndex = new Dictionary<string, Mesh>();

		public MeshManager()
		{
		}

		public Mesh Load(Stream stream)
		{
			throw new NotImplementedException();
		}

		public Mesh Load(string path)
		{
			throw new NotImplementedException();
		}

		public void Unload(Mesh asset)
		{
			throw new NotImplementedException();
		}

		public void Add(Mesh asset, string path)
		{
			if (index.ContainsKey(asset.ID))
				throw new Exception("Asset already already added in the manager! Or is it a duplicate GUID?");

			asset.ID = new Guid();

			index[asset.ID] = asset;
			pathIndex[path] = asset;
		}

	}
}
