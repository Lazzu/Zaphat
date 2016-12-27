using System;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;

namespace Zaphat.Core
{
	public class TextureSettings
	{
		public TextureWrappingMode WrapMode
		{
			get;
			set;
		}

		public TextureFilterMode FilterMode
		{
			get;
			set;
		}

		public float AnisotrophyLevel
		{
			get;
			set;
		}

		public int MipMapLevel
		{
			get;
			set;
		}

		public PixelInternalFormat Format { get; set; }

		public TextureSettings()
		{
			WrapMode = TextureWrappingMode.Repeat;
			FilterMode = TextureFilterMode.Trilinear;
			Format = PixelInternalFormat.Rgb;
			MipMapLevel = 0;
		}

		public void Apply(TextureTarget target)
		{
			var minFilter = MipMapLevel > 0 ? FilterMode.MinMip : FilterMode.Min;
			GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)minFilter);
			GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)FilterMode.Mag);
			GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)WrapMode.S);
			GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)WrapMode.T);
			GL.TexParameter(target, TextureParameterName.TextureWrapR, (int)WrapMode.R);

			if (GPUCapabilities.MaxAnisotrophy > 0)
			{
				AnisotrophyLevel = Math.Max(1.0f, Math.Min(AnisotrophyLevel, GPUCapabilities.MaxAnisotrophy));
				// TODO: OpenTK Bug: https://github.com/opentk/opentk/issues/212
				GL.TexParameter(target, (TextureParameterName)(0x84FE), AnisotrophyLevel);
				//GL.TextureParameter(textureName, (TextureParameterName)(0x84FE), AnisotrophyLevel);
			}

		}
	}
}
