using System;

namespace Zaphat.Core
{
	public static class GPUResourceManagers
	{
		private static readonly TextureNameManager textureNameManager = new TextureNameManager();

		internal static TextureNameManager TextureNameManager
		{
			get
			{
				return textureNameManager;
			}
		}
	}
}
