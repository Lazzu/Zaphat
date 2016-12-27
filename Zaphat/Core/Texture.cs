using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;

namespace Zaphat.Core
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

		public void SetTextureUnit(int unit)
		{
			GL.BindTextureUnit(unit, GLName);
			ActivateUnit(unit);
		}

		static HashSet<int> activatedUnits = new HashSet<int>();
		public static void ActivateUnit(int unit)
		{
			if (unit < 0) throw new ArgumentException("Texture unit to activate can only be 0 or larger");
			if (unit >= GPUCapabilities.MaxTextureUnits) throw new Exception(string.Format("The GPU does not support more than {0} texture units!", GPUCapabilities.MaxTextureUnits));

			if (activatedUnits.Contains(unit))
				return;

			GL.ActiveTexture(TextureUnit.Texture0 + unit);
			activatedUnits.Add(unit);
		}
	}
}
