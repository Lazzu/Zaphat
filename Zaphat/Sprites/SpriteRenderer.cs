
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;
using Zaphat.Rendering;
using System.Collections.Generic;

namespace Zaphat.Sprites
{
	public class SpriteRenderer
	{
		private class BatchKey
		{
			public ShaderProgram Shader { get; private set; }

			public Texture Texture { get; private set; }

			public BatchKey(ShaderProgram program, Texture texture)
			{
				Shader = program;
				Texture = texture;
			}

			public override bool Equals(object obj)
			{
				var tmp = obj as BatchKey;
				return tmp != null && Equals(tmp);
			}

			public bool Equals(BatchKey obj)
			{
				return obj.Shader == Shader && obj.Texture == Texture;
			}

			public override int GetHashCode()
			{
				return Shader.GetHashCode() ^ Texture.GetHashCode();
			}
		}

		private readonly ArrayBufferVector2 _spriteBuffer;
		private readonly ElementArrayBuffer<uint> _indices;
		private readonly VertexArrayObject _vao;
		private readonly VertexAttribute _vattrib;
		private Buffer<GPUSprite> _instanceBuffer;

		BatchKey _batchKey = null;
		Dictionary<BatchKey, List<GPUSprite>> _batchData = new Dictionary<BatchKey, List<GPUSprite>>();

		public SpriteRenderer()
		{
			_spriteBuffer = new ArrayBufferVector2(BufferUsageHint.StaticDraw)
			{
				Data = new[]
				{
					new Vector2(0f, 0f),
					new Vector2(1f, 0f),
					new Vector2(0f, 1f),
					new Vector2(1f, 1f),
				}
			};

			_indices = new ElementArrayBuffer<uint>(BufferUsageHint.StaticDraw)
			{
				Data = new uint[]
				{
					0, 1, 2,
					1, 2, 3,
				}
			};

			_vao = new VertexArrayObject();

			_vattrib = new VertexAttribute("vertex", 2, VertexAttribPointerType.Float, BlittableValueType.StrideOf(_spriteBuffer.Data), 0);

			_vao.Bind();
			_spriteBuffer.Bind();
			_indices.Bind();
			_vao.UnBind();

		}

		public void RenderSprite(Sprite sprite, Vector3 position, Vector3 scale, Quaternion rotation)
		{
			// TODO: Decide if shader supports batching or immediate drawing. Now support only batch drawing.
			RenderSpriteBatched(sprite, position, scale, rotation);
		}

		public void RenderSpriteImmediate(Sprite sprite, Vector3 position, Vector3 scale, Quaternion rotation)
		{
			sprite.Shader.Use();

			sprite.Shader.SendUniform("Position", ref position);
			sprite.Shader.SendUniform("Scale", ref scale);
			sprite.Shader.SendUniform("Rotation", ref rotation);

			_vao.Bind();

			_vattrib.Set(sprite.Shader);

			GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}

		public void RenderSpriteBatched(Sprite sprite, Vector3 position, Vector3 scale, Quaternion rotation)
		{
			// Produce a key for the batch
			var key = new BatchKey(sprite.Shader, sprite.Texture);

			// Check if the given sprite fits in the current batch
			if (_batchKey != null && _batchKey != key)
			{
				// Render out the current batch
				RenderBatch();
			}

			List<GPUSprite> batch = null;

			// Set up current batch
			if (_batchKey == null)
			{
				_batchKey = key;
				if (!_batchData.TryGetValue(key, out batch))
				{
					batch = new List<GPUSprite>();
					_batchData.Add(key, batch);
				}
			}

			if (batch == null)
				throw new Exception("Batch is null!");

			// Add the sprite to the batch
			batch.Add(new GPUSprite()
				{
					Position = position,
					Scale = scale,
					Rotation = rotation,
					TexCoords = sprite.TexCoords,
					Color = Vector4.One,
				});
		}

		public void RenderBatch()
		{
			// Get the current batch data
			var batch = _batchData[_batchKey];

			// Create the buffer if it didn't yet exist
			if (_instanceBuffer == null)
			{
				_instanceBuffer = new Buffer<GPUSprite>(BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
				_instanceBuffer.ShadowStore = false;
				_instanceBuffer.Bind();

				// Set up attrib locations

				var texCoordLocation = _batchKey.Shader.GetAttribLocation("TexCoord");
				var colorLocation = _batchKey.Shader.GetAttribLocation("Color");
				var rotationLocation = _batchKey.Shader.GetAttribLocation("Rotation");
				var positionLocation = _batchKey.Shader.GetAttribLocation("Position");
				var scaleLocation = _batchKey.Shader.GetAttribLocation("Scale");

				_instanceBuffer.BindVertexAttribArrayBuffer(texCoordLocation, 4, 0, VertexAttribPointerType.Float);
				_instanceBuffer.BindVertexAttribArrayBuffer(colorLocation, 4, 4 * sizeof(float), VertexAttribPointerType.Float);
				_instanceBuffer.BindVertexAttribArrayBuffer(rotationLocation, 4, 8 * sizeof(float), VertexAttribPointerType.Float);
				_instanceBuffer.BindVertexAttribArrayBuffer(positionLocation, 3, 12 * sizeof(float), VertexAttribPointerType.Float);
				_instanceBuffer.BindVertexAttribArrayBuffer(scaleLocation, 3, 15 * sizeof(float), VertexAttribPointerType.Float);

				_instanceBuffer.VertexAttribDivisor(texCoordLocation, 1);
				_instanceBuffer.VertexAttribDivisor(colorLocation, 1);
				_instanceBuffer.VertexAttribDivisor(rotationLocation, 1);
				_instanceBuffer.VertexAttribDivisor(positionLocation, 1);
				_instanceBuffer.VertexAttribDivisor(scaleLocation, 1);
			}

			// Bind and update the buffer
			_instanceBuffer.Bind();
			_instanceBuffer.Upload(batch.ToArray());

			_vattrib.Set(_batchKey.Shader);

			// The final drawcall
			GL.DrawElementsInstanced(PrimitiveType.TriangleStrip, 4, DrawElementsType.UnsignedInt, IntPtr.Zero, batch.Count);

			// Reset the current batch so a new one will be started
			batch.Clear();
			_batchKey = null;
		}
	}
}