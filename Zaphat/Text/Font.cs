using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using OpenTK;
using Zaphat.Assets.Textures;
using Zaphat.Core;
using Zaphat.Utilities;

namespace Zaphat.Text
{
	public class Font
	{
		public string Face
		{
			get;
			protected set;
		}
		public Texture Atlas
		{
			get;
			protected set;
		}

		public int LineHeight
		{
			get;
			protected set;
		}

		public int Base
		{
			get;
			protected set;
		}

		public int ScaleW
		{
			get;
			protected set;
		}

		public int ScaleH
		{
			get;
			protected set;
		}

		public int Pages
		{
			get;
			protected set;
		}

		public Vector4 Padding
		{
			get;
			protected set;
		}

		public Glyph DefaultGlyph
		{
			get;
			protected set;
		}

		List<Glyph> Glyphs;
		Dictionary<char, Glyph> GlyphCache = new Dictionary<char, Glyph>();

		List<Kerning> Kernings;
		Dictionary<int, Dictionary<int, int>> KerningCache = new Dictionary<int, Dictionary<int, int>>();

		public Font(string face, List<Glyph> glyphs, List<Kerning> kernings, Texture atlas, int lineHeight, int baseValue, int scaleW, int scaleH, int pages, char defaultCharacter, Vector4 padding)
		{
			Face = face;
			Glyphs = glyphs;
			Kernings = kernings;
			Atlas = atlas;
			LineHeight = lineHeight;
			Base = baseValue;
			ScaleW = scaleW;
			ScaleH = scaleH;
			Pages = pages;
			Padding = padding;

			WarmupKerningCache();
			WarmupGlyphCache();

			Glyph tmp;
			if (!GlyphCache.TryGetValue(defaultCharacter, out tmp))
			{
				tmp = Glyphs[0];
			}

			DefaultGlyph = tmp;
		}

		public void GetGlyphVertexData(char c, out Vector4 pos, out Vector4 uv, out float xAdvance)
		{
			var g = GetGlyph(c);
			if (g == null)
			{
				g = DefaultGlyph;
			}
			GetGlyphVertexData(g, out pos, out uv, out xAdvance);
		}

		public void GetGlyphVertexData(Glyph g, out Vector4 pos, out Vector4 uv, out float xAdvance)
		{
			var sizex = 1f / ScaleW;
			var sizey = 1f / ScaleH;

			xAdvance = g.XAdvance * sizex;

			pos = new Vector4(-0.5f, -0.5f, 0.5f, 0.5f);
			pos *= new Vector4(g.Width, g.Height, g.Width, g.Height);
			pos += Padding;
			pos += new Vector4(g.XOffset, g.YOffset, g.XOffset, g.YOffset);
			pos *= new Vector4(sizex, sizey, sizex, sizey);

			uv = new Vector4(g.X, g.Y, g.X + g.Width, g.Y + g.Height);
			//uv += new Vector4(Padding.X, Padding.W, Padding.Z, Padding.Y);
			uv += Padding;
			uv *= new Vector4(sizex, sizey, sizex, sizey);
			//uv = new Vector4(uv.X, 1.0f - uv.Y, uv.Z, 1.0f - uv.W);
		}

		public void WarmupGlyphCache()
		{
			for (int i = 0; i < Glyphs.Count; i++)
			{
				var g = Glyphs[i];
				if (GlyphCache.ContainsKey(g.Character))
				{
					Logger.Warning(string.Format("Glyph cache already has a glyph for {0}", g.Id));
					continue;
				}
				GlyphCache.Add(g.Character, g);
			}
		}

		public Glyph GetGlyph(char c)
		{
			Glyph g = null;
			GlyphCache.TryGetValue(c, out g);
			return g;
		}

		public void WarmupKerningCache()
		{
			Dictionary<int, int> tmp;

			for (int i = 0; i < Kernings.Count; i++)
			{
				var k = Kernings[i];

				if (!KerningCache.TryGetValue(k.First, out tmp))
				{
					tmp = new Dictionary<int, int>();
					KerningCache.Add(k.First, tmp);
				}

				int kVal = 0;
				if (!tmp.TryGetValue(k.Second, out kVal))
				{
					tmp.Add(k.Second, k.Amount);
				}
				else
				{
					Logger.Warning(string.Format("The kerning cache already has kerning for the pair of {0} and {1}", k.First, k.Second));
				}
			}
		}

		public int GetKerning(char a, char b)
		{
			var ag = GetGlyph(a);
			var bg = GetGlyph(b);

			if (ag == null || bg == null)
				return 0;

			return GetKerning(ag.Id, bg.Id);
		}

		public int GetKerning(int a, int b)
		{
			Dictionary<int, int> tmp;
			if (!KerningCache.TryGetValue(a, out tmp))
			{
				// No cache for it, no kerning
				return 0;
			}

			int output;
			tmp.TryGetValue(b, out output);

			return output;
		}

		public static Font Load(string path)
		{
			var mySerializer = new XmlSerializer(typeof(XmlFont));
			using (var myFileStream = new FileStream(path, FileMode.Open))
			{
				var data = (XmlFont)mySerializer.Deserialize(myFileStream);

				var chars = new List<Glyph>();
				var kernings = new List<Kerning>();

				for (int i = 0; i < data.chars.Length; i++)
				{
					var c = data.chars[i];
					chars.Add(new Glyph(c.id, c.x, c.y, c.width, c.height, c.xoffset, c.yoffset, c.xadvance, c.page));
				}

				for (int i = 0; i < data.kernings.Length; i++)
				{
					var k = data.kernings[i];
					kernings.Add(new Kerning(k.first, k.second, k.amount));
				}

				var texture = TextureManager.Global.LoadToSDF(data.pages[0].file);

				var paddingStrings = data.info.padding.Split(',');
				var padding = Vector4.Zero;

				padding.X = -Convert.ToInt32(paddingStrings[0]);
				padding.Y = -Convert.ToInt32(paddingStrings[1]);
				padding.Z = Convert.ToInt32(paddingStrings[2]);
				padding.W = Convert.ToInt32(paddingStrings[3]);

				var font = new Font(data.info.face, chars, kernings, texture, data.common.lineHeight, data.common.baseValue, data.common.scaleW, data.common.scaleH, data.common.pages, (char)0x7F, padding);

				Logger.Log(string.Format("Loaded font {0}", path));

				return font;
			}
		}

		[XmlRoot("font")]
		public class XmlFont
		{
			[XmlElement("info")]
			public FontInfo info;

			[XmlElement("common")]
			public FontCommon common;

			[XmlArray("pages")]
			[XmlArrayItem("page")]
			public FontPage[] pages;

			[XmlArray("chars")]
			[XmlArrayItem("char")]
			public FontChar[] chars;

			[XmlArray("kernings")]
			[XmlArrayItem("kerning")]
			public FontKerning[] kernings;

			[XmlType("info")]
			public class FontInfo
			{
				[XmlAttribute]
				public string face;
				[XmlAttribute]
				public int size;
				[XmlAttribute]
				public int bold;
				[XmlAttribute]
				public int italic;
				[XmlAttribute]
				public string charset;
				[XmlAttribute]
				public int stretchH;
				[XmlAttribute]
				public int smooth;
				[XmlAttribute]
				public int aa;
				[XmlAttribute]
				public string padding;
				[XmlAttribute]
				public string spacing;
				[XmlAttribute]
				public int outline;
			}
			[XmlType("common")]
			public class FontCommon
			{
				[XmlAttribute]
				public int lineHeight;
				[XmlAttribute]
				public int baseValue;
				[XmlAttribute]
				public int scaleW;
				[XmlAttribute]
				public int scaleH;
				[XmlAttribute]
				public int pages;
				[XmlAttribute]
				public int packed;
			}
			[XmlType("page")]
			public class FontPage
			{
				[XmlAttribute]
				public int id;
				[XmlAttribute]
				public string file;
			}
			[XmlType("char")]
			public class FontChar
			{
				[XmlAttribute]
				public int id;
				[XmlAttribute]
				public int x;
				[XmlAttribute]
				public int y;
				[XmlAttribute]
				public int width;
				[XmlAttribute]
				public int height;
				[XmlAttribute]
				public int xoffset;
				[XmlAttribute]
				public int yoffset;
				[XmlAttribute]
				public int xadvance;
				[XmlAttribute]
				public int page;
				[XmlAttribute]
				public int chnl;
			}
			[XmlType("kerning")]
			public class FontKerning
			{
				[XmlAttribute]
				public int first;
				[XmlAttribute]
				public int second;
				[XmlAttribute]
				public int amount;
			}
		}
	}
}
