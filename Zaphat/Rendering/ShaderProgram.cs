using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Zaphat.Rendering
{
	public class ShaderProgram
	{
		public int Name
		{
			get;
			protected set;
		}

		public ShaderProgram()
		{
			Name = GL.CreateProgram();
		}

		public void Release()
		{
			GL.DeleteProgram(Name);
		}

		public void AttachShader(Shader shader)
		{
			GL.AttachShader(Name, shader.Name);
		}

		public void AttachShaders(params Shader[] shaders)
		{
			foreach (var shader in shaders)
			{
				AttachShader(shader);
			}
		}

		public void Link()
		{
			GL.LinkProgram(Name);
		}

		public void Use()
		{
			GL.UseProgram(Name);
		}

		public void UnUse()
		{
			GL.UseProgram(0);
		}

		#region Uniforms

		Dictionary<string, int> uniforms = new Dictionary<string, int>();

		/// <summary>
		/// Tries to find the location of an uniform from this ShaderProgram object.
		/// </summary>
		/// <param name="name">Name of the uniform.</param>
		/// <returns>True if uniform was found. Otherwise false.</returns>
		public bool FindUniformLocation(string name)
		{
			int location = GetUniformLocation(name);

			// Check for valid response
			if (location == -1)
				return false;

			return true;
		}

		int GetUniformLocation(string name)
		{
			int location;
			if (!uniforms.TryGetValue(name, out location))
			{
				location = GL.GetUniformLocation(Name, name);

				uniforms.Add(name, location);
			}
			return location;
		}

		public void SendUniform(string name, float value)
		{
			int location = GetUniformLocation(name);
			GL.Uniform1(location, value);
		}

		public void SendUniform(string name, double value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}

		public void SendUniform(string name, int value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}

		public void SendUniform(string name, ref Vector2 value)
		{
			GL.Uniform2(GetUniformLocation(name), ref value);
		}

		public void SendUniform(string name, Vector2 value)
		{
			GL.Uniform2(GetUniformLocation(name), ref value);
		}

		public void SendUniform(string name, ref Vector3 value)
		{
			GL.Uniform3(GetUniformLocation(name), ref value);
		}

		public void SendUniform(string name, Vector3 value)
		{
			GL.Uniform3(GetUniformLocation(name), ref value);
		}

		public void SendUniform(string name, ref Vector4 value)
		{
			GL.Uniform4(GetUniformLocation(name), ref value);
		}

		public void SendUniform(string name, Vector4 value)
		{
			GL.Uniform4(GetUniformLocation(name), ref value);
		}

		public void SendUniform(string name, ref Matrix2 value)
		{
			GL.UniformMatrix2(GetUniformLocation(name), false, ref value);
		}

		public void SendUniform(string name, Matrix2 value)
		{
			GL.UniformMatrix2(GetUniformLocation(name), false, ref value);
		}

		public void SendUniform(string name, ref Matrix3 value)
		{
			GL.UniformMatrix3(GetUniformLocation(name), false, ref value);
		}

		public void SendUniform(string name, Matrix3 value)
		{
			GL.UniformMatrix3(GetUniformLocation(name), false, ref value);
		}

		public void SendUniform(string name, ref Matrix4 value)
		{
			GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
		}

		public void SendUniform(string name, Matrix4 value)
		{
			GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
		}

		#endregion


		Dictionary<string, int> attrLocations = new Dictionary<string, int>();

		internal int GetAttribLocation(string attribName)
		{
			int location = -1;
			if (!attrLocations.TryGetValue(attribName, out location))
			{
				location = GL.GetAttribLocation(Name, attribName);
				attrLocations.Add(attribName, location);
			}
			return location;
		}
	}
}
