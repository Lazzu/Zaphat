using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
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
				Zaphat.Utilities.Logger.Debug(info);
			}
		}

		public void ShaderSourceFile(string path)
		{
			var lines = new List<string>(File.ReadLines(path));

			lines[1] = string.Format("#line 2 \"{0}\"\n{1}", path, lines[1]);

			var source = string.Join("\n", lines);

			ShaderSource(source);
		}
	}
}
