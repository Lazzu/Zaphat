using System;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;

namespace Zaphat.Assets.Textures
{
	public class Texture : GPUResource/*, IDisposable*/
	{
		TextureSettings settings;
		bool dirtySettings;

		public TextureSettings Settings
		{
			get
			{
				return settings;
			}

			set
			{
				settings = value;
				dirtySettings = true;
			}
		}

		public TextureTarget Target
		{
			get;
			protected set;
		}

		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		public Texture(TextureTarget target)
		{
			//GLName = GPUResourceManagers.TextureNameManager.Get();
			GLName = GL.GenTexture();
			Target = target;
		}

		public void Use()
		{
			GL.BindTexture(Target, GLName);

			if (dirtySettings)
			{
				ApplySettings();
			}
		}

		public void UnBind()
		{
			GL.BindTexture(Target, 0);
		}

		public void ApplySettings()
		{
			settings.Apply(Target);
			dirtySettings = false;
		}

		public void Release()
		{
			GL.DeleteTexture(GLName);
		}

		/*
		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
		}

		void Dispose(bool manual)
		{
			GC.SuppressFinalize(this);

			if (GLName != -1)
			{
				GPUResourceManagers.TextureNameManager.Release(GLName);
			}

			GLName = -1;

			if (!manual)
			{
				Utilities.Logger.Debug(string.Format("A texture was not manually disposed! Path: {0}", Path));
			}
		}

		~Texture()
		{
			Dispose(false);
		}
		#endregion
		*/
	}
}
