using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;

namespace Zaphat.Assets.Textures
{
	public class TextureManager : IAssetManager<Texture>
	{
		Dictionary<Guid, Texture> index = new Dictionary<Guid, Texture>();
		Dictionary<string, Texture> pathIndex = new Dictionary<string, Texture>();

		public void Add(Texture asset, string path)
		{
			throw new NotImplementedException();
		}

		public Texture Load(Stream stream)
		{
			Texture tex = null;

			using (Bitmap bmp = new Bitmap(stream))
			{
				tex = new Texture(TextureTarget.Texture2D);

				PixelInternalFormat format;
				PixelFormat bmpFormat;

				switch (bmp.PixelFormat)
				{
					case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
						format = PixelInternalFormat.Rgb;
						bmpFormat = PixelFormat.Rgb;
						break;
					case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
						format = PixelInternalFormat.Rgb;
						bmpFormat = PixelFormat.Rgb;
						break;
					default:
						throw new NotSupportedException("Only RGB and RGBA image formats are supported for now!");
				}

				tex.Settings = new TextureSettings()
				{
					Format = format,
				};

				tex.Use();

				var bits = bmp.LockBits(new Rectangle(new Point(0, 0), new Size(bmp.Width, bmp.Height)), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				GL.TexImage2D(TextureTarget.Texture2D, 0, format, bmp.Width, bmp.Height, 0, bmpFormat, PixelType.Byte, bits.Scan0);

				bmp.UnlockBits(bits);

				tex.UnBind();
			}

			return tex;
		}

		public Texture Load(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Load(fs);
			}
		}

		public void Unload(Texture asset)
		{
			var id = asset.ID;
			var path = asset.Path;
			asset.Release();
			index.Remove(id);
			pathIndex.Remove(path);
		}
	}
}
