using System;
using Zaphat.Core;

namespace Zaphat.Assets.Meshes
{
	public abstract class Mesh : GPUResource
	{
		public string Name
		{
			get;
			set;
		}

		[NonSerialized]
		public bool Dirty;

		public abstract void Upload();
		public abstract void Bind();
		public abstract void BindForDrawing();
		public abstract void UnBind();

		public abstract void Draw();
	}

	public class Mesh<T, T2> : Mesh where T : struct where T2 : struct
	{
		VertexArrayObject vao = new VertexArrayObject();

		[NonSerialized]
		Buffer<T> buffer = new Buffer<T>(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer);

		[NonSerialized]
		ElementArrayBuffer<T2> indices = new ElementArrayBuffer<T2>();

		public T[] Data
		{
			get { return buffer.Data; }
			set { buffer.Data = value; Dirty = true; }
		}

		public T2[] Indices
		{
			get { return indices.Data; }
			set { indices.Data = value; }
		}

		public Mesh()
		{

		}

		public override void Upload()
		{
			buffer.Upload();
			indices.Upload();
		}

		public override void Bind()
		{
			vao.Bind();
		}

		public override void BindForDrawing()
		{
			vao.Bind();
		}

		public override void UnBind()
		{
			vao.UnBind();
		}

		public override void Draw()
		{
			throw new NotImplementedException();
		}
	}
}
