using System.Runtime.InteropServices;
using OpenTK;

namespace Zaphat.Assets.Meshes
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DefaultMeshData
	{
		public Vector4 Position;
		public Vector4 Normal;
		public Vector4 Tangent;
		public Vector4 Color;
		public Vector4 UV0;
		public Vector4 UV1;
		public Vector4 UV2;
		public Vector4 UV3;
	}
}
