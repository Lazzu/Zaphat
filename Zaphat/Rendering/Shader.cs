using System;
using System.Diagnostics;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Rendering
{
	public class Shader
	{
		public int Name
		{
			get;
			protected set;
		}

		public ShaderType Type
		{
			get;
			protected set;
		}

		public Shader(ShaderType type)
		{
			Type = type;
			Name = GL.CreateShader(type);
		}

		public Shader(ShaderType type, string source) : this(type)
		{
			ShaderSource(source);
		}

		public void Delete()
		{
			GL.DeleteShader(Name);
		}

		public void ShaderSource(string source)
		{
			//Debug.Write("Shader source:\n\n");
			//Debug.WriteLine(source);
			GL.ShaderSource(Name, source);
			Compile();
		}

		void Compile()
		{
			GL.CompileShader(Name);
			CheckCompileResult();
		}

		[Conditional("DEBUG")]
		void CheckCompileResult()
		{
			int compileResult;
			GL.GetShader(Name, ShaderParameter.CompileStatus, out compileResult);
			if (compileResult != 1)
			{
				string info;
				GL.GetShaderInfoLog(Name, out info);

				throw new Exception(info); // TODO: Replace exception with custom shader exception
			}
		}

		public void ShaderSourceFile(string path)
		{
			var source = string.Join("\n", File.ReadLines(path));
			ShaderSource(source);
		}
	}
}
