using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;


namespace Zaphat.Rendering
{
	public class ArrayBufferVector2 : Buffer<Vector2>
	{
		public ArrayBufferVector2() : base(BufferTarget.ArrayBuffer)
		{

		}

		public ArrayBufferVector2(BufferUsageHint hint) : base(BufferTarget.ArrayBuffer, hint)
		{

		}

		public void BindVertexAttrib(int attrib)
		{
			BindVertexAttribArrayBuffer(attrib, 2, 0, VertexAttribPointerType.Float);
		}
	}

	public class ArrayBufferVector3 : Buffer<Vector3>
	{
		public ArrayBufferVector3() : base(BufferTarget.ArrayBuffer)
		{

		}

		public ArrayBufferVector3(BufferUsageHint hint) : base(BufferTarget.ArrayBuffer, hint)
		{

		}

		public void BindVertexAttrib(int attrib)
		{
			BindVertexAttribArrayBuffer(attrib, 3, 0, VertexAttribPointerType.Float);
		}
	}

	public class ArrayBufferVector4 : Buffer<Vector4>
	{
		public ArrayBufferVector4() : base(BufferTarget.ArrayBuffer)
		{

		}

		public ArrayBufferVector4(BufferUsageHint hint) : base(BufferTarget.ArrayBuffer, hint)
		{

		}

		public void BindVertexAttrib(int attrib)
		{
			BindVertexAttribArrayBuffer(attrib, 4, 0, VertexAttribPointerType.Float);
		}
	}
}
