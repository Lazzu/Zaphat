using System;
using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Assets.Textures
{
	public class WrapMode
	{
		public TextureWrapMode S
		{
			get;
			protected set;
		}

		public TextureWrapMode T
		{
			get;
			protected set;
		}

		public TextureWrapMode R
		{
			get;
			protected set;
		}

		public static WrapMode Clamp = new WrapMode()
		{
			S = TextureWrapMode.ClampToEdge,
			T = TextureWrapMode.ClampToEdge,
			R = TextureWrapMode.ClampToEdge
		};

		public static WrapMode Repeat = new WrapMode()
		{
			S = TextureWrapMode.Repeat,
			T = TextureWrapMode.Repeat,
			R = TextureWrapMode.Repeat
		};

		public static WrapMode Mirror = new WrapMode()
		{
			S = TextureWrapMode.MirroredRepeat,
			T = TextureWrapMode.MirroredRepeat,
			R = TextureWrapMode.MirroredRepeat
		};
	}
}
