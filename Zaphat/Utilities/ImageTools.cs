using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using PFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace Zaphat.Utilities
{
	public static class ImageTools
	{

		static void LoadBitmap(string path, out byte[] bytes, out int Width, out int Height, out OpenTK.Graphics.OpenGL4.PixelFormat format)
		{
			using (var bmp = new Bitmap(path))
			{
				Width = bmp.Width;
				Height = bmp.Height;
				var area = new Rectangle(new Point(0, 0), new Size(bmp.Width, bmp.Height));
				var data = bmp.LockBits(area, ImageLockMode.ReadOnly, bmp.PixelFormat);
				var length = data.Stride * data.Height;
				bytes = new byte[length];
				Marshal.Copy(data.Scan0, bytes, 0, length);
				bmp.UnlockBits(data);
				switch (bmp.PixelFormat)
				{
					case PixelFormat.Format32bppArgb:
						format = PFormat.Rgba;
						break;
					case PixelFormat.Format24bppRgb:
						format = PFormat.Rgb;
						break;
					default:
						throw new System.Exception("Unsupported image format! Only 24bpp RGB and 32bpp RGBA images are supported.");
				}
			}
		}

	}
}
