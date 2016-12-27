using System;
namespace Zaphat.Text
{
	[Serializable]
	public class Glyph
	{
		public int Id
		{
			get;
			protected set;
		}

		public char Character
		{
			get
			{
				return Convert.ToChar(Id);
			}
		}

		public int X
		{
			get;
			protected set;
		}

		public int Y
		{
			get;
			protected set;
		}

		public int Width
		{
			get;
			protected set;
		}

		public int Height
		{
			get;
			protected set;
		}

		public int XOffset
		{
			get;
			protected set;
		}

		public int YOffset
		{
			get;
			protected set;
		}

		public int XAdvance
		{
			get;
			protected set;
		}

		public int Page
		{
			get;
			protected set;
		}

		public Glyph(int id, int x, int y, int w, int h, int xoff, int yoff, int xadv, int page)
		{
			Id = id;
			X = x;
			Y = y;
			Width = w;
			Height = h;
			XOffset = xoff;
			YOffset = yoff;
			XAdvance = xadv;
			Page = page;
		}
	}
}
