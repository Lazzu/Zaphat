using System;
using System.Collections.Generic;
using OpenTK;
using Zaphat.Assets.Textures;
using Zaphat.Core;

namespace Zaphat.Sprites
{
	public class SpriteBatch
	{
		List<Sprite> sprites = new List<Sprite>();
		List<Vector2> positions = new List<Vector2>();
		List<Vector2> scales = new List<Vector2>();
		List<Vector4> colors = new List<Vector4>();

		HashSet<Texture> usedTextures = new HashSet<Texture>();
		List<Texture> textures = new List<Texture>();

		public bool Dirty
		{
			get;
			set;
		}

		public ArrayBufferVector3 UVOffsetBuffer
		{
			get;
			set;
		}



		public void Add(Sprite sprite, Vector2 position, Vector2 scale, Vector4 color)
		{
			sprites.Add(sprite);
			positions.Add(position);
			scales.Add(scale);
			colors.Add(color);

			if (!usedTextures.Contains(sprite.Atlas))
			{
				usedTextures.Add(sprite.Atlas);
				textures.Add(sprite.Atlas);
			}

			Dirty = true;
		}

		public void PrepareForDrawing()
		{
			Dirty = false;


		}

		public void Draw()
		{
			if (Dirty)
			{
				PrepareForDrawing();
			}


		}

		public void Clear()
		{
			sprites.Clear();
			positions.Clear();
			scales.Clear();
			colors.Clear();
			usedTextures.Clear();
			textures.Clear();
		}
	}
}
