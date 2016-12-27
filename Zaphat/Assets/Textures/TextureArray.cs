using System;
using Zaphat.Core;

namespace Zaphat.Assets.Textures
{
	public class TextureArray : Texture
	{

		public int Layers
		{
			get;
			set;
		}

		public TextureArray() : base(OpenTK.Graphics.OpenGL4.TextureTarget.Texture2DArray)
		{

		}
	}
}
