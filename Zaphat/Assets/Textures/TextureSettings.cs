using System;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;

namespace Zaphat.Assets.Textures
{
	public class TextureSettings
	{
		public WrapMode WrapMode
		{
			get;
			set;
		}

		public TextureFilterMode FilterMode
		{
			get;
			set;
		}

		public int AnisotrophyLevel
		{
			get;
			set;
		}

		public int MipMapLevel
		{
			get;
			set;
		}

		public void Apply(TextureTarget target)
		{
			AnisotrophyLevel = Math.Min(AnisotrophyLevel, GPUCapabilities.MaxAnisotrophy);
			var minFilter = MipMapLevel > 0 ? FilterMode.MinMip : FilterMode.Min;

			GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)minFilter);
			GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)FilterMode.Mag);
			GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)WrapMode.S);
			GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)WrapMode.T);
			GL.TexParameter(target, TextureParameterName.TextureWrapR, (int)WrapMode.R);

			// TODO: OpenTK Bug: https://github.com/opentk/opentk/issues/212
			GL.TexParameter(target, (TextureParameterName)(0x84FE), AnisotrophyLevel);


		}
	}
}
