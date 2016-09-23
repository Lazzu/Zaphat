using System;
using OpenTK;
using OpenTK.Graphics;

namespace Zaphat.Application
{
	/// <summary>
	/// Base implementation of the game window. It isn't required to use this 
	/// and you can roll your own, but you have to take care of resource loading and set up.
	/// </summary>
	public class ZapApp : GameWindow
	{
		public ZapApp(int width, int height, GraphicsMode mode)
			: base(width, height, mode, "Zaphat Application", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.Default)
		{

		}

		protected override void OnLoad(EventArgs e)
		{
			// Initialize stuff

			// Load resources

			base.OnLoad(e); // Throw load event
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			// Update time and deltas and stuff

			base.OnUpdateFrame(e); // Throw updateframe event

			// Process physics etc
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			// Clear screen

			// Set up rendering queues

			base.OnRenderFrame(e); // Throw renderframe event

			// Render

			// Clear queues
		}
	}
}
