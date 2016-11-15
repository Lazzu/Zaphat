using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Assets.Textures;
using Zaphat.Core;

namespace Zaphat.Rendering
{
	public class ShaderProgram
	{
		public Guid ID { get; protected set; }
		public string Name { get; protected set; }

		public int GLName
		{
			get;
			protected set;
		}

		public ShaderProgram() : this(Guid.NewGuid(), "Unnamed") { }

		public ShaderProgram(string name) : this(Guid.NewGuid(), name) { }

		public ShaderProgram(Guid id, string name)
		{
			ID = id;
			Name = name;
			GLName = GL.CreateProgram();
		}

		public void Release()
		{
			GL.DeleteProgram(GLName);
		}

		public void AttachShader(Shader shader)
		{
			GL.AttachShader(GLName, shader.Name);
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
			GL.LinkProgram(GLName);
		}

		public void Use()
		{
			GL.UseProgram(GLName);
		}

		public void UnUse()
		{
			GL.UseProgram(0);
		}

		#region Uniforms

		Dictionary<string, int> uniforms = new Dictionary<string, int>();
		Dictionary<string, int> blocks = new Dictionary<string, int>();

		/// <summary>
		/// Tries to find the location of an uniform from this ShaderProgram object.
		/// </summary>
		/// <param name="name">Name of the uniform.</param>
		/// <returns>True if uniform was found. Otherwise false.</returns>
		public bool FindUniformLocation(string name)
		{
			int location = GetUniformLocation(name);

			// Check for valid response
			return location != -1;
		}

		/// <summary>
		/// Find the uniform block index and return if it succeeded or not
		/// </summary>
		/// <param name="name">The uniform block name</param>
		/// <returns>True if uniform block index was found. False otherwise.</returns>
		public bool FindUniformBlockIndex(string name)
		{
			int index = GetUniformBlockIndex(name);

			var found = index != -1;

			if (!found)
			{
				Utilities.Logger.Debug("Did not find uniform block \"" + name + "\" from shader \"" + Name + "\"");
			}

			return found;
		}

		int GetUniformBlockIndex(string name)
		{
			int index;
			if (!blocks.TryGetValue(name, out index))
			{
				index = GL.GetUniformBlockIndex(GLName, name);

				Zaphat.Utilities.Logger.CheckGLError(string.Format("GLName: {0}, name:{1}", GLName, name));

				if (index < 0)
				{
					Utilities.Logger.Debug("Did not find uniform block \"" + name + "\" from shader \"" + Name + "\" with GLName " + GLName + "");
				}

				blocks.Add(name, index);
			}
			return index;
		}

		int GetUniformLocation(string name)
		{
			int location;
			if (!uniforms.TryGetValue(name, out location))
			{
				location = GL.GetUniformLocation(GLName, name);

				uniforms.Add(name, location);
			}
			return location;
		}

		public void BindUniformBlock<T>(string name, UniformBufferObject<T> buffer) where T : struct
		{
			var index = GetUniformBlockIndex(name);

			if (index < 0)
				return;

			GL.UniformBlockBinding(GLName, index, buffer.BindingPoint);
		}

		public void BindTextureUnit(Texture texture, string uniform, int unit)
		{
			texture.Use();
			texture.SetTextureUnit(unit);
			Use();
			SendUniform(uniform, unit);
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
				location = GL.GetAttribLocation(GLName, attribName);
				attrLocations.Add(attribName, location);
			}
			return location;
		}
	}
}
