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

        public Texture Load(Stream stream, TextureSettings textureSettings)
        {
            Texture tex = null;

            using (Bitmap bmp = new Bitmap(stream))
            {
                tex = new Texture(TextureTarget.Texture2D)
                {
                    Settings = textureSettings,
                };

                PixelFormat bmpFormat;

                switch (bmp.PixelFormat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        bmpFormat = PixelFormat.Bgr;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                        bmpFormat = PixelFormat.Bgra;
                        break;
                    default:
                        throw new NotSupportedException("Only RGB and RGBA image formats are supported for now!");
                }

                tex.Use();

                var bits = bmp.LockBits(new Rectangle(new Point(0, 0), new Size(bmp.Width, bmp.Height)), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                GL.TexImage2D(TextureTarget.Texture2D, 0, textureSettings.Format, bmp.Width, bmp.Height, 0, bmpFormat, PixelType.UnsignedByte, bits.Scan0);

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

		public Texture Load(Stream stream)
		{
            var settings = new TextureSettings(); // Load with default settings
            return Load(stream, settings);
        }

		public Texture LoadToSDF(Stream stream)
		{
			Texture2D tex = null;

			using (Bitmap bmp = new Bitmap(stream))
			{
				tex = new Texture2D();

                tex.Settings = new TextureSettings(TextureWrappingMode.Repeat, TextureFilterMode.Trilinear, 0f, 1);

				tex.Use();

				var bytes = BitmapToBytes(bmp);

				var sdfBytes = SDFBitmapGenerator.GenerateSDF(bytes, bmp.Width, bmp.Height, Image.GetPixelFormatSize(bmp.PixelFormat) / 8);

				GL.TexImage2D(TextureTarget.Texture2D, 0, tex.Settings.Format, bmp.Width, bmp.Height, 0, PixelFormat.DepthComponent, PixelType.Float, sdfBytes);

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
