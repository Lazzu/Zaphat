using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Core
{
	/// <summary>
	/// A wrapper for OpenGL vertex array objects using Zaphat.Rendering.Buffer.
	/// </summary>
	public class VertexArrayObject
	{
		bool dirty = true;

		public int Name { get; protected set; }

		// TODO: Attributes
		/*VertexAttribute[] _attributes;
		public VertexAttribute[] Attributes
		{
			get { return _attributes; }
			set { _attributes = value; dirty = true; }
		}*/

		public VertexArrayObject()
		{
			Name = GL.GenVertexArray();
		}

		public void Bind()
		{
			GL.BindVertexArray(Name);

			// TODO: Attributes
			/*if (dirty)
			{
				for (int i = 0; i < _attributes.Length; i++)
				{

				}
			}*/
		}

		public void UnBind()
		{
			GL.BindVertexArray(0);
		}
	}
}
