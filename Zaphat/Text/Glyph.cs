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

		public float X
		{
			get;
			protected set;
		}

		public float Y
		{
			get;
			protected set;
		}

		public float Width
		{
			get;
			protected set;
		}

		public float Height
		{
			get;
			protected set;
		}

		public float XOffset
		{
			get;
			protected set;
		}

		public float YOffset
		{
			get;
			protected set;
		}

		public float XAdvance
		{
			get;
			protected set;
		}

		public int Page
		{
			get;
			protected set;
		}

		public Glyph(int id, float x, float y, float w, float h, float xoff, float yoff, float xadv, int page)
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
