using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System;

namespace Zaphat.Core
{
	public static class GPUCapabilities
	{
		static bool polled = false;
		static int maxAnisotrophy;
		static int maxTextureUnits;

		static HashSet<string> ext;

		/// <summary>
		/// What is the maximum anisotropic filtering level supported by the GPU?
		/// </summary>
		/// <value>The max anisotrophy.</value>
		public static int MaxAnisotrophy
		{
			get
			{
				PollCapabilities();
				return maxAnisotrophy;
			}
		}

		/// <summary>
		/// Does the GPU support bindless textures?
		/// </summary>
		/// <value><c>true</c> if bindless textures is supported; otherwise, <c>false</c>.</value>
		public static bool BindlessTextures
		{
			get
			{
				PollCapabilities();
				return ext.Contains("GL_ARB_bindless_textures");
			}
		}

		public static int MaxTextureUnits
		{
			get
			{
				PollCapabilities();
				return maxTextureUnits;
			}
		}

		/// <summary>
		/// Return if the GPU supports the given extension
		/// </summary>
		/// <returns><c>true</c>, if extension was supported by the GPU, <c>false</c> otherwise.</returns>
		/// <param name="extension">Extension.</param>
		public static bool HasExtension(string extension)
		{
			PollCapabilities();
			return ext.Contains(extension);
		}

		/// <summary>
		/// Find out what capabilities are supported on the GPU.
		/// </summary>
		public static void PollCapabilities()
		{
			if (polled)
				return;

			polled = true;

			ext = new HashSet<string>();

			// Get the extensions
			int n = 0;
			GL.GetInteger(GetPName.NumExtensions, out n);
			for (int i = 0; i < n; i++)
			{
				ext.Add(GL.GetString(StringNameIndexed.Extensions, i));
			}

			// Get max anisotrophy level
			// FIXME: OpenTK Bug https://github.com/opentk/opentk/issues/212
			maxAnisotrophy = GL.GetInteger((GetPName)((int)0x84FF));

			// Get max number of possible texture units
			maxTextureUnits = GL.GetInteger(GetPName.MaxTextureUnits);
		}

	}
}
