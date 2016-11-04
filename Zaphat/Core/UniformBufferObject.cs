using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	public class UniformBufferObject<T> : Buffer<T> where T : struct
	{
        int _bindingPointIndex = -1;
        public int BindingPointIndex
        {
            get
            {
                return _bindingPointIndex;
            }
            set
            {
                _bindingPointIndex = value;
                Bind();
                GL.BindBufferBase(BufferRangeTarget.UniformBuffer, _bindingPointIndex, Name);
            }
        }

        public UniformBufferObject() : base(BufferTarget.UniformBuffer) { }
	}
}
