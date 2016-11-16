using System;
using OpenTK;
using Zaphat.Assets.Textures;

namespace Zaphat.Sprites
{
	public class Sprite
	{
		public string Name
		{
			get;
			set;
		}

		public Texture Atlas
		{
			get;
			set;
		}

		public Vector3 UVOffset
		{
			get;
			set;
		}

		public Vector2 UVSize
		{
			get;
			set;
		}

		public Sprite()
		{
		}
	}
}
