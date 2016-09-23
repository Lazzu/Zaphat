using System.Collections.Generic;
using OpenTK;

namespace Zaphat.Sprites
{
	public class Sprite
	{
		#region private data

		readonly Vector2h[] quad = new Vector2h[] {
			new Vector2h(0f, 0f),
			new Vector2h(1f, 0f),
			new Vector2h(0f, 1f),
			new Vector2h(1f, 1f)
		};

		#endregion

		#region basic info

		/// <summary>
		/// Gets or sets the type of the sprite.
		/// </summary>
		/// <value>The type of the sprite.</value>
		public SpriteType Type
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the texture position of the sprite. The third component (z) is used to determine the texture array page.
		/// </summary>
		/// <value>The texture position of the sprite.</value>
		public Vector3h TexturePosition
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the size of the sprite.
		/// </summary>
		/// <value>The size of the sprite.</value>
		public Vector2h Size
		{
			get;
			set;
		}

		#endregion

		#region animation related

		/// <summary>
		/// Gets or sets the offset of a frame. Used only for animated sprites
		/// </summary>
		/// <value>The animation offset.</value>
		public Vector3h FrameOffset
		{
			get;
			set;
		}

		public int Frames
		{
			get;
			set;
		}

		public float FrameTime
		{
			get;
			set;
		}

		#endregion

		#region slicing related

		/// <summary>
		/// Gets or sets the slice offsets. XZ = top left corner, ZW = bottom right corner
		/// </summary>
		/// <value>The slice offsets.</value>
		public Vector4h SliceOffsets
		{
			get;
			set;
		}

		#endregion

		public Sprite()
		{
			Type = SpriteType.Single;
		}

		public void GenerateSingleVertices(Vector2h position, Vector2h scale, List<SpriteVertex> vertices)
		{
			var i = 0;
			for (int x = 0; x < 2; x++)
			{
				for (int y = 0; y < 2; y++)
				{
					var q = quad[i];

					vertices.Add(new SpriteVertex()
					{
						Position = new Vector2h(position.X + q.X * scale.X, position.Y + q.X * scale.X),
						UV = new Vector2h(TexturePosition.X + q.X * Size.X, TexturePosition.Y + q.X * Size.X),
					});

					i++;
				}
			}
		}
	}
}
