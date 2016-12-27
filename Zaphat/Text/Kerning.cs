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

		public int Amount
		{
			get;
			protected set;
		}

		public Kerning(int first, int second, int amount)
		{
			First = first;
			Second = second;
			Amount = amount;
		}
	}
}
