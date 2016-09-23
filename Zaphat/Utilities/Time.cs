using System;
namespace Zaphat.Utilities
{
	/// <summary>
	/// Time helper to hold the current frame time values.
	/// </summary>
	public static class Time
	{
		public static double Delta
		{
			get;
			internal set;
		}

		public static double Elapsed
		{
			get;
			internal set;
		}

		public static double PhysicsDelta
		{
			get;
			internal set;
		}

		public static double PhysicsElapsed
		{
			get;
			internal set;
		}
	}
}
