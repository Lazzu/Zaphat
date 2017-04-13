using System;
using OpenTK;
using System.Runtime.InteropServices;

namespace Zaphat.Sprites
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GPUSprite
	{
		public Vector4 TexCoords;
		public Vector4 Color;
		public Quaternion Rotation;
		public Vector3 Position;
		public Vector3 Scale;

		public static int SizeInBytes
		{
			get
			{
				unsafe
				{
					return sizeof(GPUSprite);
				}
			}
		}
	}
}

