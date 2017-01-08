using System;
using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	public class TextureFilterMode
	{
		public TextureMinFilter Min
		{
			get;
			protected set;
		}

		public TextureMinFilter MinMip
		{
			get;
			protected set;
		}

		public TextureMagFilter Mag
		{
			get;
			protected set;
		}

        protected TextureFilterMode(){}

        public static TextureFilterMode Custom(TextureMinFilter min, TextureMinFilter mip, TextureMagFilter mag)
        {
            return new TextureFilterMode()
            {
                Min = min,
                MinMip = mip,
                Mag = mag,
            };
        }

		public static TextureFilterMode Nearest = new TextureFilterMode()
		{
			Min = TextureMinFilter.Nearest,
			MinMip = TextureMinFilter.NearestMipmapNearest,
			Mag = TextureMagFilter.Nearest
		};

		public static TextureFilterMode Bilinear = new TextureFilterMode()
		{
			Min = TextureMinFilter.Linear,
			MinMip = TextureMinFilter.NearestMipmapNearest,
			Mag = TextureMagFilter.Linear
		};

		public static TextureFilterMode Trilinear = new TextureFilterMode()
		{
			Min = TextureMinFilter.Linear,
			MinMip = TextureMinFilter.LinearMipmapLinear,
			Mag = TextureMagFilter.Linear
		};

	}
}
