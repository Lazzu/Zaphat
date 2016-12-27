using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;
using Zaphat.Rendering;

namespace Zaphat.Text
{
	public class TextMesh
	{
		string _text;
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				ReconstructMesh();
			}
		}

		Font _font;
		public Font Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
				ReconstructMesh();
			}
		}

		public ShaderProgram ShaderProgram
		{
			get;
			set;
		}

		ArrayBufferVector2 vertexBuffer;
		int charactersToDraw = 0;

		VertexArrayObject vao;
		VertexAttribute positionAttribute;
		VertexAttribute texcoordAttribute;

		public TextMesh()
		{
			vao = new VertexArrayObject();

			positionAttribute = new VertexAttribute("position", 2, VertexAttribPointerType.Float, 16, 0, false);
			texcoordAttribute = new VertexAttribute("texcoord", 2, VertexAttribPointerType.Float, 16, 8, false);

			vertexBuffer = new ArrayBufferVector2(BufferUsageHint.DynamicDraw);
			vertexBuffer.ShadowStore = false;

			vertexBuffer.Bind();

			/*vertexBuffer.BindVertexAttribArrayBuffer(0, 2, 0, VertexAttribPointerType.Float);
			vertexBuffer.BindVertexAttribArrayBuffer(1, 2, Vector2.SizeInBytes, VertexAttribPointerType.Float);*/
		}

		public void ReconstructMesh()
		{
			if (string.IsNullOrEmpty(_text))
				return;

			if (_font == null)
				return;

			var chars = _text.ToCharArray();

			// TODO: Optimization: Do not use list, and use a separate vertex array and use raw uploading for the buffer
			var vertices = new List<Vector2>();

			// Temp values
			Vector4 pos;
			Vector4 uv;
			var prevChar = (char)0;
			float kerning;
			float xAdv;

			// How much the character has offset in the mesh?
			float xAdvance = 0;
			float yAdvance = _font.Base / (float)_font.ScaleH;

			for (int i = 0; i < chars.Length; i++)
			{
				char c = chars[i];

				switch (c)
				{
					// TODO: Special cases for \n, space, tab, etc..

					// \n
					case (char)13:
						xAdvance = 0;
						yAdvance = _font.LineHeight / (float)_font.ScaleH;
						break;
					// Space bar
					case (char)32:
						_font.GetGlyphVertexData(c, out pos, out uv, out xAdv);
						kerning = _font.GetKerning(prevChar, c) / (float)_font.ScaleW;
						xAdvance += xAdv + kerning;
						break;
					// Normal text, add it to the vertices
					default:
						_font.GetGlyphVertexData(c, out pos, out uv, out xAdv);

						kerning = _font.GetKerning(prevChar, c) / (float)_font.ScaleW;

						vertices.Add(new Vector2(pos.X + xAdvance + kerning, pos.Y + yAdvance));
						vertices.Add(new Vector2(uv.X, uv.Y));

						vertices.Add(new Vector2(pos.Z + xAdvance + kerning, pos.W + yAdvance));
						vertices.Add(new Vector2(uv.Z, uv.W));

						vertices.Add(new Vector2(pos.Z + xAdvance + kerning, pos.Y + yAdvance));
						vertices.Add(new Vector2(uv.Z, uv.Y));

						vertices.Add(new Vector2(pos.X + xAdvance + kerning, pos.Y + yAdvance));
						vertices.Add(new Vector2(uv.X, uv.Y));

						vertices.Add(new Vector2(pos.X + xAdvance + kerning, pos.W + yAdvance));
						vertices.Add(new Vector2(uv.X, uv.W));

						vertices.Add(new Vector2(pos.Z + xAdvance + kerning, pos.W + yAdvance));
						vertices.Add(new Vector2(uv.Z, uv.W));

						xAdvance += xAdv;

						charactersToDraw++;

						break;
				}

				prevChar = c;
			}

			vertexBuffer.Data = vertices.ToArray();
		}

		public void Draw()
		{
			ShaderProgram.Use();

			vao.Bind();
			vertexBuffer.BindForDrawing();

			positionAttribute.Set(ShaderProgram);
			texcoordAttribute.Set(ShaderProgram);
			ShaderProgram.BindTextureUnit(_font.Atlas, "sdfTexture", 0);

			GL.DrawArrays(PrimitiveType.Triangles, 0, charactersToDraw * 6);
		}
	}
}
