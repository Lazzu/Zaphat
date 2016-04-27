using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Zaphat.Rendering
{
    /// <summary>
    /// Low-level vertex batch for very basic rendering. Works almost like the immediate mode. 
    /// Renders everything right after the batch has been finished. Very inefficient but good for basic stuff.
    /// </summary>
    public class VertexBatch
    {
        List<Vector3> vertices;
        List<Vector3> normals;
        List<Vector4> colors;
        List<Vector2> texcoords;

        PrimitiveType _type;

        int vbo = -1;

        /// <summary>
        /// Bagin a batch with specified polygon mode.
        /// </summary>
        /// <param name="mode"></param>
        public void Begin(PrimitiveType type )
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            colors = new List<Vector4>();
            texcoords = new List<Vector2>();
            _type = type;
        }

        public void AddVertices(IEnumerable<Vector3> v)
        {
            if (vertices == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            vertices.AddRange(v);
        }

        public void AddNormals(IEnumerable<Vector3> n)
        {
            if (normals == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            normals.AddRange(n);
        }

        public void AddColors(IEnumerable<Vector4> c)
        {
            if (colors == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            colors.AddRange(c);
        }

        public void AddTexcoords(IEnumerable<Vector2> tc)
        {
            if (texcoords == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            texcoords.AddRange(tc);
        }

        public void AddVertex(Vector3 position)
        {
            if (vertices == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            vertices.Add(position);
        }

        public void AddNormal(Vector3 normal)
        {
            if (normals == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            normals.Add(normal);
        }

        public void AddColor(Vector4 color)
        {
            if (colors == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            colors.Add(color);
        }

        public void AddTexcoord(Vector2 texcoord)
        {
            if (texcoords == null)
                throw new Exception("You must call VertexBatch.Begin before you can add data to the batch!");

            texcoords.Add(texcoord);
        }

        public void End()
        {
            var count = vertices.Count;
            
            switch(_type)
            {
                case PrimitiveType.Points:
                    if (count < 1)
                        throw new Exception("You need to specify at least one vertex when rendering points.");
                    break;
                case PrimitiveType.Lines:
                    if (count < 2)
                        throw new Exception("You need to specify at least two vertices when rendering lines.");
                    if (count % 2 != 0)
                        throw new Exception("You need to have even number of vertices when rendering lines!");
                    break;
                case PrimitiveType.LineLoop:
                case PrimitiveType.LineStrip:
                    if (count < 2)
                        throw new Exception("You need to specify at least two vertices when rendering lines.");
                    break;
                case PrimitiveType.Triangles:
                    if (count < 3 || count % 3 != 0)
                        throw new Exception("You need exactly three vertices for each single triangle.");
                    break;
                case PrimitiveType.TriangleFan:
                case PrimitiveType.TriangleStrip:
                    if (count < 3)
                        throw new Exception("You need to specify at least three vertices when rendering triangles.");
                    break;
            }
            
            if (normals.Count > 0 && normals.Count != count)
                throw new Exception("Vertex count and normals count do not match!");

            if (colors.Count > 0 && colors.Count != count)
                throw new Exception("Vertex count and colors count do not match!");

            if (texcoords.Count > 0 && texcoords.Count != count)
                throw new Exception("Vertex count and texcoords count do not match!");

            if (vbo < 0)
            {
                GL.GenBuffers(1, out vbo);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            
            List<float> buffer = new List<float>();

            foreach (var item in vertices)
            {
                buffer.Add(item.X);
                buffer.Add(item.Y);
                buffer.Add(item.Z);
            }
            foreach (var item in normals)
            {
                buffer.Add(item.X);
                buffer.Add(item.Y);
                buffer.Add(item.Z);
            }
            foreach (var item in colors)
            {
                buffer.Add(item.X);
                buffer.Add(item.Y);
                buffer.Add(item.Z);
                buffer.Add(item.W);
            }
            foreach (var item in texcoords)
            {
                buffer.Add(item.X);
                buffer.Add(item.Y);
            }

            GL.BufferData(BufferTarget.ArrayBuffer, count, buffer.ToArray(), BufferUsageHint.StreamDraw);
            GL.DrawArrays(_type, 0, count);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.Flush();

            vertices = null;
            normals = null;
            colors = null;
            texcoords = null;
        }
    }
}
