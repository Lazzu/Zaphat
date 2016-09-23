using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	/// <summary>
	/// A wrapper for OpenGL vertex array objects using Zaphat.Rendering.Buffer.
	/// </summary>
	public class VertexArrayObject
	{
		public int Name { get; protected set; }

		public VertexArrayObject()
		{
			Name = GL.GenVertexArray();
		}

		public void Bind()
		{
			GL.BindVertexArray(Name);
		}

		public void UnBind()
		{
			GL.BindVertexArray(0);
		}
	}
}
