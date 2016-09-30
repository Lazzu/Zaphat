using System.Runtime.InteropServices;
using OpenTK;
namespace Zaphat.Core
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DefaultCameraData
	{
		public Matrix4 ViewProjection;
		public Matrix3 NormalMatrix;
	}
}
