using System.Runtime.InteropServices;
using OpenTK;

namespace Zaphat.Core
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DefaultTransformData
	{
		public Vector4 Position;
		public Quaternion Rotation;
		public Vector4 Scale;
		public Matrix4 ViewProjection;
	}
}
