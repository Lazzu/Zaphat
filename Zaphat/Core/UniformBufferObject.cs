using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	public class UniformBufferObject<T> : Buffer<T> where T : struct
	{
		int _bindingPoint;

		public int BindingPoint
		{
			get { return _bindingPoint; }
			set
			{
				_bindingPoint = value;
				GL.BindBufferBase(BufferRangeTarget.UniformBuffer, _bindingPoint, Name);
			}
		}

		public UniformBufferObject() : base(BufferTarget.UniformBuffer) { }
	}
}
