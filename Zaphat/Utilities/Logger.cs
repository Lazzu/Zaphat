using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

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

		public static void CheckGLError(string debugText = "")
		{
			ErrorCode error = GL.GetError();

			if (error == ErrorCode.NoError)
				return;

			for (; error != ErrorCode.NoError; error = GL.GetError())
			{
				switch (error)
				{
					case ErrorCode.ContextLost:
						Debug("Context lost! " + debugText, 1);
						break;
					case ErrorCode.InvalidEnum:
						Debug("Invalid Enum! " + debugText, 1);
						break;
					case ErrorCode.InvalidFramebufferOperation:
						Debug("Invalid framebuffer operation! " + debugText, 1);
						break;
					case ErrorCode.InvalidOperation:
						Debug("Invalid operation! " + debugText, 1);
						break;
					case ErrorCode.InvalidValue:
						Debug("Invalid value! " + debugText, 1);
						break;
					case ErrorCode.OutOfMemory:
						Debug("Out of memory! " + debugText, 1);
						break;
					case ErrorCode.TableTooLarge:
						Debug("Table too large! " + debugText, 1);
						break;
					case ErrorCode.TextureTooLargeExt:
						Debug("Texture too large! " + debugText, 1);
						break;
				}
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

		//[Conditional("DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Debug(string text, int traceSkipOffset = 0)
		{
			var trace = new StackTrace(1 + traceSkipOffset, true);
			System.Diagnostics.Debug.WriteLine("[{0}][Debug] {1}\nStack Trace:\n{2}\n\n", CurrentTime, text, trace.ToString());
		}

		//[Conditional("DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void VerboseDebug(string text)
		{
			if (!Verbose)
				return;

			Debug(text);
		}
	}
}
