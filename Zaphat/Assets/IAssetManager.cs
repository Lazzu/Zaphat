using System;
using System.IO;

namespace Zaphat.Assets
{
	public interface IAssetManager<T>
	{
		T Load(string path);
		T Load(Stream stream);
		void Unload(T asset);
	}
}
