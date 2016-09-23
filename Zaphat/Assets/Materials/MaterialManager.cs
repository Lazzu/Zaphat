using System;
using System.Collections.Generic;

namespace Zaphat.Assets.Materials
{
	public class MaterialManager
	{
		Dictionary<string, Material> Materials = new Dictionary<string, Material>();

		public Material this[string index]
		{
			get
			{
				Material mat = null;
				if (Materials.TryGetValue(index, out mat))
					return mat;
				return null;
			}
			set
			{
				if (Materials.ContainsKey(index))
				{
					Utilities.Logger.Debug(string.Format("Replacing Material {0} in MaterialManager", index));
				}
				Materials[index] = value;
			}
		}


	}
}
