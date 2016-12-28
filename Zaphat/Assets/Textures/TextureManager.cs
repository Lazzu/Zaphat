using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;
using Zaphat.Utilities;

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
						bmpFormat = PixelFormat.Bgr;
						break;
					case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
						format = PixelInternalFormat.Rgba;
						bmpFormat = PixelFormat.Bgra;
						break;
					default:
						throw new NotSupportedException("Only RGB and RGBA image formats are supported for now!");
				}

				tex.Settings = new TextureSettings()
				{
					Format = format,
					MipMapLevel = 1,
				};

				tex.Use();

				var bits = bmp.LockBits(new Rectangle(new Point(0, 0), new Size(bmp.Width, bmp.Height)), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

				GL.TexImage2D(TextureTarget.Texture2D, 0, format, bmp.Width, bmp.Height, 0, bmpFormat, PixelType.UnsignedByte, bits.Scan0);

				if (tex.Settings.MipMapLevel > 0)
				{
					GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
				}

				bmp.UnlockBits(bits);

				tex.UnBind();
			}

			Logger.Log(string.Format("Loaded texture {0}", tex.GLName));

			return tex;
		}

		public Texture LoadToSDF(Stream stream)
		{
			Texture2D tex = null;

			using (Bitmap bmp = new Bitmap(stream))
			{
				tex = new Texture2D();

				PixelInternalFormat format;
				PixelFormat bmpFormat;

				switch (bmp.PixelFormat)
				{
					case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
						format = PixelInternalFormat.Rgb;
						bmpFormat = PixelFormat.Bgr;
						break;
					case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
						format = PixelInternalFormat.Rgba;
						bmpFormat = PixelFormat.Bgra;
						break;
					default:
						throw new NotSupportedException("Only RGB and RGBA image formats are supported for now!");
				}

				tex.Settings = new TextureSettings()
				{
					Format = format,
					MipMapLevel = 1,
				};

				tex.Use();

				var bytes = BitmapToBytes(bmp);

				var sdfBytes = SDFBitmapGenerator.GenerateSDF(bytes, bmp.Width, bmp.Height, Image.GetPixelFormatSize(bmp.PixelFormat) / 8);

				GL.TexImage2D(TextureTarget.Texture2D, 0, format, bmp.Width, bmp.Height, 0, bmpFormat, PixelType.UnsignedByte, sdfBytes);

				if (tex.Settings.MipMapLevel > 0)
				{
					GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
				}

				tex.UnBind();
			}

			Logger.Log(string.Format("Loaded SDF texture {0}", tex.GLName));

			return tex;
		}

		byte[] BitmapToBytes(Bitmap bmp)
		{
			using (var stream = new MemoryStream())
			{
				bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
				return stream.ToArray();
			}
		}

		public Texture Load(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Load(fs);
			}
		}

		public Texture LoadToSDF(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return LoadToSDF(fs);
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

		public static TextureManager Global = new TextureManager();
	}
}
