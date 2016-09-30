using System;
using System.Collections.Generic;
using System.IO;
using Assimp;
using Assimp.Configs;
using Zaphat.Utilities;

namespace Zaphat.Assets.Meshes
{
	public class MeshManager : IAssetManager<Mesh>
	{
		Dictionary<Guid, Mesh> index = new Dictionary<Guid, Mesh>();
		Dictionary<string, Mesh> pathIndex = new Dictionary<string, Mesh>();

		AssimpContext importer = new AssimpContext();

		public MeshManager()
		{
			importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
		}

		public Mesh Load(Stream stream)
		{
			throw new NotImplementedException();
		}

		public void ImportAssimp(string path, List<Mesh> meshes, List<Zaphat.Assets.Materials.Material> materials)
		{
			var scene = importer.ImportFile(path, PostProcessPreset.TargetRealTimeMaximumQuality);

			for (int i = 0; i < scene.Meshes.Count; i++)
			{
				var mesh = scene.Meshes[i];

				var importedMesh = Load(mesh);

				Add(importedMesh, path + "/" + mesh.Name);

				meshes.Add(importedMesh);
			}

			/*for (int i = 0; i < scene.Materials.Count; i++)
			{
				var material = scene.Materials[i];

				materials.Add(null);
			}*/
		}

		public Mesh Load(string path)
		{
			throw new NotImplementedException();
		}

		Mesh Load(Assimp.Mesh mesh)
		{
			if (mesh.HasVertices)
				throw new Exception("Loading mesh does not support meshes with no vertices");

			if (mesh.HasBones)
			{
				Logger.Debug("Imported model has bones but importer does not support them yet.");
			}

			var createdMesh = new Mesh<DefaultMeshData, uint>();

			var data = new DefaultMeshData[mesh.Vertices.Count];

			for (int i = 0; i < mesh.Vertices.Count; i++)
			{
				var p = mesh.Vertices[i];
				data[i].Position = new OpenTK.Vector4(p.X, p.Y, p.Z, 1);

				if (mesh.HasNormals)
				{
					var n = mesh.Normals[i];
					data[i].Normal = new OpenTK.Vector4(n.X, n.Y, n.Z, 0);
				}

				if (mesh.HasTangentBasis)
				{
					var t = mesh.Tangents[i];
					data[i].Tangent = new OpenTK.Vector4(t.X, t.Y, t.Z, 0);
				}

				if (mesh.HasVertexColors(0))
				{
					var c = mesh.VertexColorChannels[0][i];
					data[i].Color = new OpenTK.Vector4(c.R, c.G, c.B, c.A);
				}

				if (mesh.HasTextureCoords(0))
				{
					var uv = mesh.TextureCoordinateChannels[0][i];
					data[i].UV0 = new OpenTK.Vector4(uv.X, uv.Y, uv.Z, 0);
				}

				if (mesh.HasTextureCoords(1))
				{
					var uv = mesh.TextureCoordinateChannels[1][i];
					data[i].UV1 = new OpenTK.Vector4(uv.X, uv.Y, uv.Z, 0);
				}

				if (mesh.HasTextureCoords(2))
				{
					var uv = mesh.TextureCoordinateChannels[2][i];
					data[i].UV2 = new OpenTK.Vector4(uv.X, uv.Y, uv.Z, 0);
				}

				if (mesh.HasTextureCoords(3))
				{
					var uv = mesh.TextureCoordinateChannels[3][i];
					data[i].UV3 = new OpenTK.Vector4(uv.X, uv.Y, uv.Z, 0);
				}

			}

			createdMesh.Data = data;
			createdMesh.ID = new Guid();
			createdMesh.Name = mesh.Name;

			index[createdMesh.ID] = createdMesh;

			return createdMesh;
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
