using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	public class UniformBufferObject<T> : Buffer<T> where T : struct
	{
		public UniformBufferObject() : base(BufferTarget.UniformBuffer) { }
	}
}
