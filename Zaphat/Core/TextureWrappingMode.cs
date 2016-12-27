using System;
using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	public class TextureWrappingMode
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

		public static TextureWrappingMode Clamp = new TextureWrappingMode()
		{
			S = TextureWrapMode.ClampToEdge,
			T = TextureWrapMode.ClampToEdge,
			R = TextureWrapMode.ClampToEdge
		};

		public static TextureWrappingMode Repeat = new TextureWrappingMode()
		{
			S = TextureWrapMode.Repeat,
			T = TextureWrapMode.Repeat,
			R = TextureWrapMode.Repeat
		};

		public static TextureWrappingMode Mirror = new TextureWrappingMode()
		{
			S = TextureWrapMode.MirroredRepeat,
			T = TextureWrapMode.MirroredRepeat,
			R = TextureWrapMode.MirroredRepeat
		};
	}
}
