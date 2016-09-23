using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Zaphat.Utilities
{
	public static class Logger
	{
		/// <summary>
		/// Gets or sets a value indicating whether logging should be verbose.
		/// </summary>
		/// <value><c>true</c> if verbose; otherwise, <c>false</c>.</value>
		static public bool Verbose
		{
			get;
			set;
		}

		static DateTime prevTime;
		static string prevTimeString;
		static string CurrentTime
		{
			get
			{
				var current = DateTime.Now;
				if (current != prevTime)
				{
					prevTimeString = current.ToShortTimeString();
					prevTime = current;
				}
				return prevTimeString;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Log(string text)
		{
			Console.WriteLine("[{0}][Log] {1}", CurrentTime, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void VerboseLog(string text)
		{
			if (!Verbose)
				return;

			Console.WriteLine("[{0}][Log] {1}", CurrentTime, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Warning(string text)
		{
			Console.WriteLine("[{0}][Warning] {1}", CurrentTime, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Error(string text)
		{
			Console.WriteLine("[{0}][Error] {1}", CurrentTime, text);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Exception(Exception e)
		{
			Console.WriteLine("[{0}][Exception] {1}", CurrentTime, e);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Custom(string text, string tag)
		{
			Console.WriteLine("[{0}][{2}] {1}", CurrentTime, text, tag);
		}

		[Conditional("DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Debug(string text)
		{
			var trace = new StackTrace();
			System.Diagnostics.Debug.WriteLine("[{0}][Debug] {1}\nStack Trace:\n{2}", CurrentTime, text, trace.ToString());
		}

		[Conditional("DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void VerboseDebug(string text)
		{
			if (!Verbose)
				return;

			Debug(text);
		}
	}
}
