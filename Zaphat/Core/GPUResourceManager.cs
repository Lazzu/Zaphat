
namespace Zaphat.Core
{
	public abstract class GPUResourceManager
	{
		public abstract int Get();
		public abstract void Get(int count, int[] generated);
		public abstract void Release(int asset);
		public abstract void Release(int[] assets);
		public abstract void Update();
	}
}
