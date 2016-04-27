using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;


namespace Zaphat.Rendering
{
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
}
