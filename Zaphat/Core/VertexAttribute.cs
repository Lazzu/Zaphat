using System;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Rendering;
using Zaphat.Utilities;

namespace Zaphat.Core
{
	public class VertexAttribute
	{
		public readonly string Name;
		public readonly int Size;
		public readonly VertexAttribPointerType Type;
		public readonly bool Normalize;
		public readonly int Stride;
		public readonly int Offset;

		private int location = -1;

		bool yelledAboutLocation = false;

		public VertexAttribute(string name, int size, VertexAttribPointerType type,
			int stride, int offset, bool normalize = false)
		{
			this.Name = name;
			this.Size = size;
			this.Type = type;
			this.Stride = stride;
			this.Offset = offset;
			this.Normalize = normalize;
		}

		public void Set(ShaderProgram program)
		{
			location = program.GetAttribLocation(this.Name);

			if (location < 0)
			{
				if (!yelledAboutLocation)
				{
					Logger.Debug(string.Format("Did not find location \"{1}\" for attribute from program {0}", program.Name, this.Name));
					yelledAboutLocation = true;
				}
				return;
			}

			yelledAboutLocation = false;

			// enable and set attribute
			GL.EnableVertexAttribArray(location);
			GL.VertexAttribPointer(location, this.Size, this.Type,
				this.Normalize, this.Stride, this.Offset);
		}
	}
}
