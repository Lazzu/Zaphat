using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Zaphat.Core
{
	public abstract class Buffer
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
		public int Name { get; protected set; }

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
		/// The amount of bytes the buffer has reserved from GPU.
		/// </summary>
		protected int _ReservedBytes = 0;

		/// <summary>
		/// If you want to store all the buffered data in the CPU memory within the buffer object Data property, set this to true.
		/// </summary>
		public bool ShadowStore = false;

		/// <summary>
		/// Upload the whole buffer to the GPU.
		/// </summary>
		public abstract void Upload();

		/// <summary>
		/// Upload a range of data to the GPU. This should be used when you don't need to upload all the data in the buffer to the GPU.
		/// </summary>
		/// <param name="from">The index to start the upload from</param>
		/// <param name="count">The amount of elements to upload.</param>
		public abstract void UploadRange(int from, int count);

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
			Zaphat.Utilities.Logger.CheckGLError();
			GL.VertexAttribPointer(attrib, elements, type, false, Stride, offset);
			Zaphat.Utilities.Logger.CheckGLError();
		}

		/// <summary>
		/// Bind the buffer for usage.
		/// </summary>
		public void Bind()
		{
			GL.BindBuffer(Target, Name);
			Zaphat.Utilities.Logger.CheckGLError();
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
			Zaphat.Utilities.Logger.CheckGLError();
		}

		/// <summary>
		/// Reserve memory from the GPU for the amount of elements of type T given.
		/// </summary>
		/// <param name="elements">The amount of elements.</param>
		public void CleanAndReserveGPUMemExactly(int elements)
		{
			_Reserved = true;
			_ReservedBytes = elements * ElementSizeInBytes;
			GL.BufferData(Target, _ReservedBytes, IntPtr.Zero, BufferUsageHint);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("ReserveGPUMem elements:{0}, _ReservedBytes:{1}", elements, _ReservedBytes));
		}

		public void CleanAndReserveGPUMemAtLeast(int elements)
		{
			var bytes = elements * ElementSizeInBytes;

			if (_ReservedBytes >= bytes)
				return;

			GL.BufferData(Target, bytes, IntPtr.Zero, BufferUsageHint);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("ReserveAtLeast elements:{0}, bytes:{1}", elements, bytes));
			_Reserved = true;
			_ReservedBytes = bytes;
		}

		/// <summary>
		/// Get a pointer to the GPU memory for more direct access.
		/// </summary>
		/// <param name="access">The access level requested. Defaults to WriteOnly.</param>
		/// <returns>The pointer to the GPU memory</returns>
		public IntPtr Map(BufferAccess access)
		{
			var returnValue = GL.MapBuffer(Target, access);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("Target:{0}, access:{1}", Target, access));
			return returnValue;
		}

		/// <summary>
		/// Release the pointer to the GPU memory to allow rendering.
		/// </summary>
		public void UnMap()
		{
			GL.UnmapBuffer(Target);
			Dirty = false;
			Zaphat.Utilities.Logger.CheckGLError(string.Format("Target:{0}", Target));
		}

		/// <summary>
		/// Clear the buffer. Also zeroes out the memory in the GPU but keeps it reserved.
		/// </summary>
		public abstract void Clear();

		/// <summary>
		/// Upload raw data to the GPU to the beginning of the buffer.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="bytes">Data size in bytes</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void UploadRaw<T>(T[] data, int bytes) where T : struct
		{
			if (_ReservedBytes < bytes)
			{
				GL.BufferData(Target, bytes, IntPtr.Zero, BufferUsageHint);
				_ReservedBytes = bytes;
			}

			UploadRangeRaw(data, 0, bytes);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("UploadRaw to:{0}", bytes));
		}

		/// <summary>
		/// Uploads raw data to the GPU using a range.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="fromBytes">The first byte offset.</param>
		/// <param name="bytes">How many bytes to upload.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void UploadRangeRaw<T>(T[] data, int fromBytes, int bytes) where T : struct
		{
			GL.BufferSubData(Target, (IntPtr)(fromBytes), bytes, data);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("UploadRangeRaw from:{0}, to:{1}", fromBytes, bytes));
		}
	}

	/// <summary>
	/// Represents a generic buffer object to transfer data to and from the GPU. Basically a wrapper around OpenGL Buffer Objects.
	/// </summary>
	/// <typeparam name="T">Data type. Must be a value-type.</typeparam>
	public class Buffer<T> : Buffer where T : struct
	{
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
		/// Create a buffer with spesific target.
		/// </summary>
		/// <param name="target">The buffer target to use.</param>
		public Buffer(BufferTarget target)
		{
			var tmpElement = default(T);
			ElementSizeInBytes = Marshal.SizeOf(tmpElement);
			Stride = BlittableValueType.StrideOf(tmpElement);
			BufferUsageHint = BufferUsageHint.StaticDraw;

			Target = target;
			int name;
			GL.GenBuffers(1, out name);

			Zaphat.Utilities.Logger.CheckGLError();

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
		/// Upload the whole buffer to the GPU from the shadow storage. If the shadow storage is empty, does nothing.
		/// </summary>
		public override void Upload()
		{
			if (!Dirty)
				return;

			Dirty = false;

			if (_data.Length == 0)
				return;

			var bytes = _data.Length * ElementSizeInBytes;

			_Reserved = _Reserved && bytes == _ReservedBytes;

			if (!_Reserved)
			{
				CleanAndReserveGPUMemExactly(bytes);
			}

			GL.BufferSubData(Target, IntPtr.Zero, _ReservedBytes, _data);

			if (!ShadowStore)
			{
				_data = null;
			}
		}

		public void Upload(T data)
		{
			Dirty = false;

			if (ShadowStore)
			{
				_data = new[] { data };
			}

			_Reserved = _Reserved && ElementSizeInBytes == _ReservedBytes;

			if (!_Reserved)
			{
				CleanAndReserveGPUMemExactly(1);
			}

			GL.BufferSubData(Target, IntPtr.Zero, _ReservedBytes, new[] { data });
			Zaphat.Utilities.Logger.CheckGLError(string.Format("Upload(T data) _Reserved:{0}", _ReservedBytes));
		}

		public void Upload(ref T data)
		{
			Dirty = false;

			if (ShadowStore)
			{
				_data = new[] { data };
			}

			_Reserved = _Reserved && ElementSizeInBytes == _ReservedBytes;

			if (!_Reserved)
			{
				CleanAndReserveGPUMemExactly(1);
			}

			GL.BufferSubData(Target, IntPtr.Zero, _ReservedBytes, new[] { data });
			Zaphat.Utilities.Logger.CheckGLError(string.Format("Upload(ref T data) _Reserved:{0}", _ReservedBytes));
		}

		/// <summary>
		/// Upload an array of data to the GPU.
		/// </summary>
		/// <param name="data">The uploaded data.</param>
		public void Upload(T[] data)
		{
			Dirty = false;

			var bytes = data.Length * ElementSizeInBytes;

			if (ShadowStore)
			{
				_data = data;
			}

			_Reserved = _Reserved && bytes == _ReservedBytes;

			if (!_Reserved)
			{
				CleanAndReserveGPUMemExactly(data.Length);
			}

			GL.BufferSubData(Target, IntPtr.Zero, _ReservedBytes, data);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("Upload(T[] data) _Reserved:{0}", _ReservedBytes));
		}

		/// <summary>
		/// Upload a range of data to the GPU from the shadow storage. This should be used when you only need to update part of the data.
		/// Note that this can be a bit slow as the default generic buffer copies the range of data in a temporary array.
		/// </summary>
		/// <param name="from">The index to start the upload from</param>
		/// <param name="count">The amount of elements to upload.</param>
		public override void UploadRange(int from, int count)
		{
			Dirty = false; // Assuming user knows what's best

			T[] tmp = new T[count];
			Array.Copy(_data, from, tmp, 0, count);

			GL.BufferSubData(Target, (IntPtr)(from * ElementSizeInBytes), count * ElementSizeInBytes, _data);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("UploadRange(int from, int count) from:{0}, count:{1}", from, count));
		}

		/// <summary>
		/// Upload a range of data to the GPU. This should be used when you only need to update part of the data.
		/// Note that this function will be very slow if shadow storage is enabled and range exceeds the shadow storage
		/// size.
		/// </summary>
		/// <param name="data">The data to be uploaded.</param>
		/// <param name="from">The index to start the upload from</param>
		/// <param name="count">The amount of elements to upload.</param>
		public void UploadRange(T[] data, int from, int count)
		{
			Dirty = false;

			if (ShadowStore)
			{
				if (_data.Length < from + count)
				{
					var tmp = new T[from + count];
					Array.Copy(_data, 0, tmp, 0, _data.Length);
					_data = tmp;
				}
				Array.Copy(data, 0, _data, from, count);
			}

			GL.BufferSubData(Target, (IntPtr)(from * ElementSizeInBytes), count * ElementSizeInBytes, data);
			Zaphat.Utilities.Logger.CheckGLError(string.Format("UploadRange(T[] data, int from, int count) from:{0}, count:{1}", from, count));
		}

		/// <summary>
		/// Clear the buffer. Also zeroes out the memory in the GPU but keeps it reserved.
		/// </summary>
		public override void Clear()
		{
			if (_data.Length > 0)
				CleanAndReserveGPUMemExactly(_data.Length);

			_data = null;
		}
	}
}
