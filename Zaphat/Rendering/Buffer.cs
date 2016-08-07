using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Zaphat.Rendering
{
	/// <summary>
	/// Represents a generic buffer object to transfer data to and from the GPU. Basically a wrapper around OpenGL Buffer Objects.
	/// </summary>
	/// <typeparam name="T">Data type. Must be a value-type.</typeparam>
	public class Buffer<T> where T : struct
	{
		/// <summary>
		/// The buffer target.
		/// </summary>
		public BufferTarget Target { get; protected set; }

		/// <summary>
		/// The buffer usage hint.
		/// </summary>
		public BufferUsageHint BufferUsageHint { get; set; }

		/// <summary>
		/// The GLName of the buffer object.
		/// </summary>
		public uint Name { get; protected set; }

		/// <summary>
		/// The backing field for Data property.
		/// </summary>
		protected T[] _data;

		/// <summary>
		/// The data array. When setter is called and when AutoUpload is true, it will be uploaded to the GPU. Otherwise Upload() or UploadRange() must be called.
		/// </summary>
		public T[] Data
		{
			get { return _data; }
			set
			{
				_Reserved = _Reserved && _data.Length == value.Length;
				_data = value;
				Dirty = true;
			}
		}

		/// <summary>
		/// True when the buffer needs be uploaded to the GPU. False otherwise.
		/// </summary>
		public bool Dirty { get; protected set; }

		/// <summary>
		/// The size of one buffer element in bytes. Will be initialized when the buffer is created.
		/// </summary>
		public int ElementSizeInBytes { get; protected set; }

		/// <summary>
		/// The stride of the buffer elements. Will be initialized when the buffer is created.
		/// </summary>
		public int Stride { get; protected set; }

		/// <summary>
		/// True if the buffer has allocated enough memory from the GPU. False otherwise.
		/// </summary>
		protected bool _Reserved = false;

		/// <summary>
		/// Create a buffer with spesific target.
		/// </summary>
		/// <param name="target">The buffer target to use.</param>
		public Buffer(BufferTarget target)
		{
			var tmpElement = default(T);
			ElementSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(tmpElement);
			Stride = BlittableValueType.StrideOf(tmpElement);
			BufferUsageHint = BufferUsageHint.StaticDraw;

			Target = target;
			uint name;
			GL.GenBuffers(1, out name);

			if (name == 0)
				throw new GraphicsException("Could not generate buffer.");

			Name = name;
			Dirty = true;
		}

		/// <summary>
		/// Create a buffer with spesific target and usage hint.
		/// </summary>
		/// <param name="target">The buffer target to use.</param>
		/// <param name="hint">The buffer usage hint to use.</param>
		public Buffer(BufferTarget target, BufferUsageHint hint) : this(target)
		{
			BufferUsageHint = hint;
		}

		/// <summary>
		/// Upload the whole buffer to the GPU.
		/// </summary>
		public void Upload()
		{
			if (!Dirty)
				return;

			Dirty = false;

			if (!_Reserved)
			{
				GL.BufferData(Target, _data.Length * ElementSizeInBytes, _data, BufferUsageHint);
				_Reserved = true;
			}
			else
			{
				GL.BufferSubData(Target, IntPtr.Zero, _data.Length * ElementSizeInBytes, _data);
			}
		}

		/// <summary>
		/// Upload a range of data to the GPU. This should be used when you don't need to upload all the data in the buffer to the GPU.
		/// </summary>
		/// <param name="from">The index to start the upload from</param>
		/// <param name="count">The amount of elements to upload.</param>
		public void UploadRange(int from, int count)
		{
			Dirty = false; // Assuming user knows what's best

			T[] tmp = new T[count];
			Array.Copy(_data, from, tmp, 0, count);

			GL.BufferSubData(Target, (IntPtr)(from * ElementSizeInBytes), count * ElementSizeInBytes, _data);
		}

		/// <summary>
		/// Enable and bind given vertex attribute pointer
		/// </summary>
		/// <param name="attrib"></param>
		/// <param name="elements"></param>
		/// <param name="type"></param>
		public void BindVertexAttribArrayBuffer(int attrib, int elements, int offset, VertexAttribPointerType type)
		{
			if (Target != BufferTarget.ArrayBuffer)
				throw new GraphicsException("Setting vertex attribute array buffer pointer is only supported for BufferTarget.ArrayBuffer");

			GL.EnableVertexAttribArray(attrib);
			GL.VertexAttribPointer(attrib, elements, type, false, Stride, offset);
		}

		/// <summary>
		/// Bind the buffer for usage.
		/// </summary>
		public void Bind()
		{
			GL.BindBuffer(Target, Name);
		}

		/// <summary>
		/// Binds the buffer and uploads if data has changed.
		/// </summary>
		public void BindForDrawing()
		{
			Bind();
			Upload();
		}

		/// <summary>
		/// Unbind the buffer from usage.
		/// </summary>
		public void UnBind()
		{
			GL.BindBuffer(Target, 0);
		}

		/// <summary>
		/// Reserve memory from the GPU for the amount of elements of type T given.
		/// </summary>
		/// <param name="elements">The amount of elements.</param>
		public void Reserve(int elements)
		{
			GL.BufferData(Target, elements * ElementSizeInBytes, IntPtr.Zero, BufferUsageHint);
			_Reserved = true;
		}

		/// <summary>
		/// Get a pointer to the GPU memory for more direct access.
		/// </summary>
		/// <param name="access">The access level requested. Defaults to WriteOnly.</param>
		/// <returns>The pointer to the GPU memory</returns>
		public IntPtr Map(BufferAccess access = BufferAccess.WriteOnly)
		{
			return GL.MapBuffer(Target, access);
		}

		/// <summary>
		/// Release the pointer to the GPU memory to allow rendering.
		/// </summary>
		public void UnMap()
		{
			GL.UnmapBuffer(Target);
			Dirty = false;
		}

		/// <summary>
		/// Clear the buffer. Also zeroes out the memory in the GPU but keeps it reserved.
		/// </summary>
		public void Clear()
		{
			if (_data.Length > 0)
				Reserve(_data.Length);

			_data = null;
		}
	}
}
