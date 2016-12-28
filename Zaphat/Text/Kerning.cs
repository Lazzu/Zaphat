using System;
namespace Zaphat.Text
{
	public class Kerning
	{
		public int First
		{
			get;
			protected set;
		}

		public int Second
		{
			get;
			protected set;
		}

		public float Amount
		{
			get;
			protected set;
		}

		public Kerning(int first, int second, float amount)
		{
			First = first;
			Second = second;
			Amount = amount;
		}
	}
}
