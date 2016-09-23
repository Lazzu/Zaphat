using System;
using System.Collections.Generic;

namespace Zaphat.Assets
{
	public abstract class Asset
	{
		public Guid ID { get; set; }
		public string Path { get; set; }
	}
}
