using System.Collections.Concurrent;
using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	/// <summary>
	/// Texture manager. Creates and releases OpenGL Texture names.
	/// </summary>
	internal class TextureNameManager : GPUResourceManager
	{
		/// <summary>
		/// The released textures.
		/// </summary>
		readonly ConcurrentQueue<int> Released = new ConcurrentQueue<int>();

		/// <summary>
		/// Get a texture handle from the GPU. 
		/// </summary>
		/// <returns>The texture.</returns>
		public override int Get()
		{
			return GL.GenTexture();
		}

		/// <summary>
		/// Get many texture handles from the gpu
		/// </summary>
		/// <param name="count">The count of textures to get.</param>
		/// <param name="textures">The array of texture handles generated.</param>
		public override void Get(int count, int[] textures)
		{
			GL.GenTextures(count, textures);
		}

		/// <summary>
		/// Release a texture handle. Can be called from any thread.
		/// </summary>
		/// <returns>The texture to be released.</returns>
		/// <param name="texture">Texture.</param>
		public override void Release(int texture)
		{
			Released.Enqueue(texture);
		}

		/// <summary>
		/// Release many texture handles. Can be called from any thread.
		/// </summary>
		/// <returns>The textures to be released.</returns>
		/// <param name="textures">Textures.</param>
		public override void Release(int[] textures)
		{
			for (int i = 0; i < textures.Length; i++)
			{
				Released.Enqueue(textures[i]);
			}
		}

		/// <summary>
		/// Update the manager. Mainly delete textures marked to be deleted.
		/// </summary>
		public override void Update()
		{
			int texture;
			while (Released.TryDequeue(out texture))
			{
				GL.DeleteTexture(texture);
			}
		}
	}
}
