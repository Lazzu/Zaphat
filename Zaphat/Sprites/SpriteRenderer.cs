
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;
using Zaphat.Rendering;

namespace Zaphat.Sprites
{
    public class SpriteRenderer
    {
        private readonly ArrayBufferVector2 _spriteBuffer;
        private readonly ElementArrayBuffer<uint> _indices;
        private readonly VertexArrayObject _vao;
        private readonly VertexAttribute _vattrib;

        private ShaderProgram _shader;

        public SpriteRenderer(ShaderProgram shader)
        {
            _shader = shader;

            _spriteBuffer = new ArrayBufferVector2(BufferUsageHint.StaticDraw)
            {
                Data = new[]
                {
                    new Vector2(0f, 0f),
                    new Vector2(1f, 0f),
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f),
                }
            };

            _indices = new ElementArrayBuffer<uint>(BufferUsageHint.StaticDraw)
            {
                Data = new uint[]
                {
                    0, 1, 2,
                    1, 2, 3,
                }
            };

            _vao = new VertexArrayObject();

            _vattrib = new VertexAttribute("vertex", 2, VertexAttribPointerType.Float, BlittableValueType.StrideOf(_spriteBuffer.Data), 0 );

            _vao.Bind();
            _spriteBuffer.Bind();
            _vattrib.Set(shader);
            _indices.Bind();
            _vao.UnBind();

        }

        public void RenderSprite(Sprite sprite, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            _shader.Use();

            _shader.SendUniform("Position", ref position);
            _shader.SendUniform("Scale", ref scale);
            _shader.SendUniform("Rotation", ref rotation);

            _vao.Bind();
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);

        }
    }
}