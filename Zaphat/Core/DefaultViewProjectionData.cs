using OpenTK;
using System.Runtime.InteropServices;

namespace Zaphat.Core
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DefaultViewProjectionData
	{
		public Matrix4 ViewProjection;
		public Matrix4 View;
		public Matrix4 Projection;
		public Matrix4 InvView;
		public Vector4 CameraWorldPosition;
		public Vector4 CameraWorldDirection;
	}
}
