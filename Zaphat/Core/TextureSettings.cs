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
			protected set;
		}

		public TextureFilterMode FilterMode
		{
			get;
			protected set;
		}

		public float AnisotrophyLevel
		{
			get;
			protected set;
		}

		public int MipMapLevel
		{
			get;
			protected set;
		}

		public PixelInternalFormat Format { get; set; }

		public TextureSettings()
		{
			WrapMode = TextureWrappingMode.Clamp;
			FilterMode = TextureFilterMode.Trilinear;
			Format = PixelInternalFormat.Rgba;
			MipMapLevel = 10;
		    AnisotrophyLevel = 16.0f;
		}

        public TextureSettings(TextureWrappingMode wrap, TextureFilterMode filter, float aniso, int mip)
        {
            WrapMode = wrap;
            FilterMode = filter;
            AnisotrophyLevel = aniso;
            MipMapLevel = mip;
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
