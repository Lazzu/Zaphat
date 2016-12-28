using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;
using Zaphat.Rendering;

namespace Zaphat.Text
{
	public class TextMesh
	{
		[StructLayout(LayoutKind.Sequential)]
		struct Vertex
		{
			Vector2 Pos;
			Vector2 UV;

			public Vertex(Vector2 pos, Vector2 uv)
			{
				Pos = pos;
				UV = uv;
			}
		}

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

		Buffer<Vertex> vertexBuffer;
		int charactersToDraw = 0;

		VertexArrayObject vao;
		VertexAttribute positionAttribute;
		VertexAttribute texcoordAttribute;

		public TextMesh()
		{
			vao = new VertexArrayObject();

			var stride = BlittableValueType.StrideOf(new Vertex());
			positionAttribute = new VertexAttribute("position", 2, VertexAttribPointerType.Float, stride, 0, false);
			texcoordAttribute = new VertexAttribute("texcoord", 2, VertexAttribPointerType.Float, stride, Vector2.SizeInBytes, false);

			vertexBuffer = new Buffer<Vertex>(BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);
			vertexBuffer.ShadowStore = false;
		}

		public void ReconstructMesh()
		{
			if (string.IsNullOrEmpty(_text))
				return;

			if (_font == null)
				return;

			var chars = _text.ToCharArray();

			// TODO: Optimization: Do not use list, and use a separate vertex array and use raw uploading for the buffer
			var vertices = new List<Vertex>();

			// Temp values
			Vector4 pos;
			Vector4 uv;
			var prevChar = (char)0;
			float kerning = 0;
			float xAdv = 0;

			var scale2 = new Vector2(1.0f / _font.ScaleW, 1.0f / _font.ScaleH);
			var scale4 = new Vector4(scale2.X, scale2.Y, scale2.X, scale2.Y);

			// How much the character has offset in the mesh?
			float xAdvance = 0;
			float yAdvance = _font.Base;

			charactersToDraw = 0;

			for (int i = 0; i < chars.Length; i++)
			{
				char c = chars[i];

				switch (c)
				{
					// TODO: Special cases for \n, space, tab, etc..

					// \n
					case (char)13:
						xAdvance = 0;
						yAdvance += _font.LineHeight;
						break;
					// Space bar
					case (char)32:
						_font.GetGlyphVertexData(c, out pos, out uv, out xAdv);
						kerning = _font.GetKerning(prevChar, c);
						xAdvance += xAdv + kerning;
						break;
					// Normal text, add it to the vertices
					default:
						_font.GetGlyphVertexData(c, out pos, out uv, out xAdv);

						kerning = _font.GetKerning(prevChar, c);

						var xGlyphOffset = xAdvance + kerning;
						var yGlyphOffset = yAdvance;

						pos.X += xGlyphOffset;
						pos.Y += yGlyphOffset;
						pos.Z += xGlyphOffset;
						pos.W += yGlyphOffset;

						pos *= scale4;
						uv *= scale4;

						// Vertices
						var v0 = new Vertex(pos.Xy, uv.Xy);
						var v1 = new Vertex(pos.Xw, uv.Xw);
						var v2 = new Vertex(pos.Zy, uv.Zy);
						var v3 = new Vertex(pos.Zw, uv.Zw);

						// Add two triangles
						vertices.Add(v0);
						vertices.Add(v1);
						vertices.Add(v2);

						vertices.Add(v1);
						vertices.Add(v2);
						vertices.Add(v3);

						// Advance
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

			positionAttribute.Set(ShaderProgram.GetAttribLocation("position"));
			texcoordAttribute.Set(ShaderProgram.GetAttribLocation("texcoord"));

			ShaderProgram.BindTextureUnit(_font.Atlas, "sdfTexture", 0);

			GL.DrawArrays(PrimitiveType.Triangles, 0, charactersToDraw * 6);
		}
	}
}
