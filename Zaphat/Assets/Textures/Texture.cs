using System;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;

namespace Zaphat.Assets.Textures
{
	public class Texture : GPUResource
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

		public void ApplySettings()
		{
			settings.Apply(Target);
			dirtySettings = false;
		}

		public void Release()
		{
			GL.DeleteTexture(GLName);
		}
	}
}
