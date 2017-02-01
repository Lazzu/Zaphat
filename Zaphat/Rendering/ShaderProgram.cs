using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;
using Zaphat.Utilities;

namespace Zaphat.Rendering
{
	public class ShaderProgram
	{
		public Guid Id { get; protected set; }
		public string Name { get; protected set; }

		public int GlName
		{
			get;
			protected set;
		}

		public ShaderProgram() : this(Guid.NewGuid(), "Unnamed") { }

		public ShaderProgram(string name) : this(Guid.NewGuid(), name) { }

		public ShaderProgram(Guid id, string name)
		{
			Id = id;
			Name = name;
			GlName = GL.CreateProgram();
		}

		public void Release()
		{
			GL.DeleteProgram(GlName);
		}

		public void AttachShader(Shader shader)
		{
			GL.AttachShader(GlName, shader.Name);
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
			GL.LinkProgram(GlName);

			Logger.CheckGLError($"Linking program {Name}");
			Logger.Log($"Linked program {Name}");
		}

		public void Use()
		{
			GL.UseProgram(GlName);
		}

		public void UnUse()
		{
			GL.UseProgram(0);
		}

		#region Uniforms

	    private readonly Dictionary<string, int> _uniforms = new Dictionary<string, int>();
	    private readonly Dictionary<string, int> _blocks = new Dictionary<string, int>();

		/// <summary>
		/// Tries to find the location of an uniform from this ShaderProgram object.
		/// </summary>
		/// <param name="name">Name of the uniform.</param>
		/// <returns>True if uniform was found. Otherwise false.</returns>
		public bool FindUniformLocation(string name)
		{
			var location = GetUniformLocation(name);

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
			var index = GetUniformBlockIndex(name);

			var found = index != -1;

			if (!found)
			{
				Logger.Debug("Did not find uniform block \"" + name + "\" from shader \"" + Name + "\"");
			}

			return found;
		}

	    private int GetUniformBlockIndex(string name)
		{
			int index;
		    if (_blocks.TryGetValue(name, out index))
		        return index;

		    index = GL.GetUniformBlockIndex(GlName, name);

		    Logger.CheckGLError($"GLName: {GlName}, name:{name}");

		    if (index < 0)
		    {
		        Logger.Debug("Did not find uniform block \"" + name + "\" from shader \"" + Name + "\" with GLName " + GlName + "");
		    }

		    _blocks.Add(name, index);
		    return index;
		}

	    private int GetUniformLocation(string name)
		{
			int location;
		    if (_uniforms.TryGetValue(name, out location))
		        return location;

		    location = GL.GetUniformLocation(GlName, name);

		    _uniforms.Add(name, location);
		    return location;
		}

		public void BindUniformBlock<T>(string name, UniformBufferObject<T> buffer) where T : struct
		{
			var index = GetUniformBlockIndex(name);

			if (index < 0)
				return;

			GL.UniformBlockBinding(GlName, index, buffer.BindingPoint);
		}

		public void BindTextureUnit(Texture texture, string uniform, int unit)
		{
			texture.SetTextureUnit(unit);
			texture.Use();
			Use();
			SendUniform(uniform, unit);
		}

		public void SendUniform(string name, float value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
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

        public void SendUniform(string name, Quaternion value)
        {
            GL.Uniform4(GetUniformLocation(name), new Vector4(value.Xyz, value.W));
        }

        public void SendUniform(string name, ref Quaternion value)
        {
            GL.Uniform4(GetUniformLocation(name), new Vector4(value.Xyz, value.W));
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


	    private readonly Dictionary<string, int> _attrLocations = new Dictionary<string, int>();

		internal int GetAttribLocation(string attribName)
		{
			int location;
		    if (_attrLocations.TryGetValue(attribName, out location))
		        return location;
		    location = GL.GetAttribLocation(GlName, attribName);
		    _attrLocations.Add(attribName, location);
		    Logger.CheckGLError("Attrib location");
		    return location;
		}
	}
}
