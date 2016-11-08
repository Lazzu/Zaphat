using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaphat.Core
{
	public class DefaultViewProjectionBuffer : UniformBufferObject<DefaultViewProjectionData>
	{
		public DefaultViewProjectionBuffer()
		{
			BufferUsageHint = OpenTK.Graphics.OpenGL4.BufferUsageHint.StreamDraw;
			ShadowStore = false;
			Data = new DefaultViewProjectionData()
			{
				View = Matrix4.Identity,
				Projection = Matrix4.Identity,
				ViewProjection = Matrix4.Identity,
				InvView = Matrix4.Identity,
				CameraWorldDirection = new Vector4(0f, 0f, 1f, 0f),
				CameraWorldPosition = new Vector4(0f, 0f, 0f, 1f)
			};
		}

		public new DefaultViewProjectionData Data;

		public void UpdateData()
		{
			Bind();
			Upload(ref Data);
		}

		public void UpdateData(DefaultViewProjectionData data)
		{
			Data = data;
			UpdateData();
		}

		/// <summary>
		/// Calculate all required fields and upload them to the buffer.
		/// </summary>
		/// <param name="view">The view matrix (camera matrix)</param>
		/// <param name="projection">The projection matrix</param>
		/// <param name="cameraWorldPos">The camera position in world</param>
		/// <param name="cameraWorldDirection">The camera direction vector in world</param>
		public void Update(Matrix4 view, Matrix4 projection, Vector3 cameraWorldPos, Vector3 cameraWorldDirection)
		{
			var vpMatrix = view * projection;
			var invView = view.Inverted();

			var d = new float[]
			{
				//cameraWorldPos.X, cameraWorldPos.Y, cameraWorldPos.Z, 1.0f,

				view.M11, view.M12, view.M13, view.M14,
				view.M21, view.M22, view.M23, view.M24,
				view.M31, view.M32, view.M33, view.M34,
				view.M41, view.M42, view.M43, view.M44,

				projection.M11, projection.M12, projection.M13, projection.M14,
				projection.M21, projection.M22, projection.M23, projection.M24,
				projection.M31, projection.M32, projection.M33, projection.M34,
				projection.M41, projection.M42, projection.M43, projection.M44,

				invView.M11, invView.M12, invView.M13, invView.M14,
				invView.M21, invView.M22, invView.M23, invView.M24,
				invView.M31, invView.M32, invView.M33, invView.M34,
				invView.M41, invView.M42, invView.M43, invView.M44,

				vpMatrix.M11, vpMatrix.M12, vpMatrix.M13, vpMatrix.M14,
				vpMatrix.M21, vpMatrix.M22, vpMatrix.M23, vpMatrix.M24,
				vpMatrix.M31, vpMatrix.M32, vpMatrix.M33, vpMatrix.M34,
				vpMatrix.M41, vpMatrix.M42, vpMatrix.M43, vpMatrix.M44,

				cameraWorldPos.X, cameraWorldPos.Y, cameraWorldPos.Z, 1.0f,

				cameraWorldDirection.X, cameraWorldDirection.Y, cameraWorldDirection.Z, 0.0f
			};

			Zaphat.Utilities.Logger.Log(view.ToString());

			/*Data.View = view;
			Data.Projection = projection;
			Data.ViewProjection = projection * view;
			Data.InvView = view.Inverted();
			Data.CameraWorldPosition = new Vector4(cameraWorldPos, 1.0f);
			Data.CameraWorldDirection = new Vector4(cameraWorldDirection, 0.0f);*/

			Bind();
			CleanAndReserveGPUMemAtLeast(1);
			UploadRangeRaw(d, 0, ElementSizeInBytes);
			//UploadRangeRaw(new[] { Data }, 0, ElementSizeInBytes);
		}
	}
}
