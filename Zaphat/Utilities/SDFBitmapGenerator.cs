using OpenTK;
using System;

namespace Zaphat.Utilities
{
	public static class SDFBitmapGenerator
	{
		static Vector2d inside = Vector2d.One * 9999;
		static Vector2d empty = Vector2d.Zero;

		static int W, H;

		/// <summary>
		/// Generates the sdf map from the given byte map.
		/// </summary>
		/// <returns>The sdf map with equal amount of channels as the source image.</returns>
		/// <param name="data">Data to read from.</param>
		/// <param name="w">The width of the map.</param>
		/// <param name="h">The height of the map.</param>
		/// <param name="bpp">Bytes per pixel.</param>
		/// <param name="channelOffset">Channel offset of which colour channel to read from.</param>
		public static byte[] GenerateSDF(byte[] data, int w, int h, int bpp, int channelOffset = 0)
		{
			if (bpp < 1 || bpp > 4)
				throw new ArgumentException("Invalid BPP", nameof(bpp));

			if (channelOffset < 0 || channelOffset >= bpp)
				throw new ArgumentException("Channel offset can not be less than zero or more than or equal to BPP", nameof(channelOffset));

			W = w;
			H = h;

			// Create grids and initialize data
			var grid1 = new Vector2d[w * h];
			var grid2 = new Vector2d[w * h];
			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					var p = y * h + x;
					if (data[y * h * bpp + x * bpp + channelOffset] < 128)
					{
						grid1[p] = inside;
						grid2[p] = empty;
					}
					else
					{
						grid1[p] = empty;
						grid2[p] = inside;
					}
				}
			}

			GenerateSDF(grid1, w, h);
			GenerateSDF(grid2, w, h);

			var output = new byte[w * h * bpp];

			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					// Calculate the actual distance from the dx/dy
					var dist1 = Get(grid1, x, y).Length;
					var dist2 = Get(grid2, x, y).Length;
					var dist = dist1 - dist2;

					// Clamp and scale it, just for display purposes.
					var c = dist * 3 + 128;
					if (c < 0) c = 0;
					if (c > 255) c = 255;

					var p = (y * h * bpp + x * bpp);

					for (int i = 0; i < bpp; i++)
					{
						output[p + i] = (byte)c;
					}
				}
			}

			return output;
		}

		static void GenerateSDF(Vector2d[] g, int w, int h)
		{
			// Pass 0
			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					var p = Get(g, x, y);
					Compare(g, ref p, x, y, -1, 0);
					Compare(g, ref p, x, y, 0, -1);
					Compare(g, ref p, x, y, -1, -1);
					Compare(g, ref p, x, y, 1, -1);
					Put(g, x, y, p);
				}

				for (int x = w - 1; x >= 0; x--)
				{
					var p = Get(g, x, y);
					Compare(g, ref p, x, y, 1, 0);
					Put(g, x, y, p);
				}
			}

			// Pass 1
			for (int y = h - 1; y >= 0; y--)
			{
				for (int x = w - 1; x >= 0; x--)
				{
					var p = Get(g, x, y);
					Compare(g, ref p, x, y, 1, 0);
					Compare(g, ref p, x, y, 0, 1);
					Compare(g, ref p, x, y, -1, 1);
					Compare(g, ref p, x, y, 1, 1);
					Put(g, x, y, p);
				}

				for (int x = 0; x < w; x++)
				{
					var p = Get(g, x, y);
					Compare(g, ref p, x, y, -1, 0);
					Put(g, x, y, p);
				}
			}
		}

		static void Put(Vector2d[] g, int x, int y, Vector2d p)
		{
			g[y * H + x] = p;
		}

		static Vector2d Get(Vector2d[] g, int x, int y)
		{
			// OPTIMIZATION: you can skip the edge check code if you make your grid
			// have a 1-pixel gutter.
			if (x >= 0 && y >= 0 && x < W && y < H)
				return g[W * y + x];

			return empty;
		}

		static void Compare(Vector2d[] g, ref Vector2d p, int x, int y, int offx, int offy)
		{
			var other = Get(g, x + offx, y + offy);
			other.X += offx;
			other.Y += offy;

			if (other.LengthSquared < p.LengthSquared)
				p = other;
		}
	}
}
