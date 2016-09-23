using OpenTK.Graphics;
using System;
using Zaphat.Application;

namespace ZaphatDevProgram
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			using (var app = new DevApp(1280, 720, new GraphicsMode(new ColorFormat(32), 32, 0, 0, 0, 2, false)))
			{
				app.Run(60, 60);
			}

		}
	}
}
