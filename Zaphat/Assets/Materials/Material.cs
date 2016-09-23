using System;
using Zaphat.Core;

namespace Zaphat.Assets.Materials
{
	[Serializable]
	public abstract class Material
	{
		public string Name
		{
			get;
			protected set;
		}

		[NonSerialized]
		public bool Dirty;

		public abstract void Upload();
	}

	[Serializable]
	public abstract class Material<T> : Material where T : struct
	{
		[NonSerialized]
		UniformBufferObject<T> buffer = new UniformBufferObject<T>();

		T _data;

		public T Data
		{
			get { return _data; }
			set { _data = value; Dirty = true; }
		}

		public override void Upload()
		{
			if (!Dirty)
				return;
			buffer.Upload(new T[] { Data });
			Dirty = false;
		}
	}
}
