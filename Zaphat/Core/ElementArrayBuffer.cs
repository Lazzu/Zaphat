using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	/// <summary>
	/// An Element Array Buffer to store mesh indices.
	/// </summary>
	/// <typeparam name="T">Must be a numeric type (byte, short, ushort, int, uint, long, ulong)</typeparam>
	public class ElementArrayBuffer<T> : Buffer<T> where T : struct
	{
		public ElementArrayBuffer() : base(BufferTarget.ElementArrayBuffer) { }

		public ElementArrayBuffer(BufferUsageHint hint) : base(BufferTarget.ElementArrayBuffer, hint) { }
	}
}
