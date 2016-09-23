using OpenTK;
namespace Zaphat.Core
{
	public class DefaultTransformBuffer : UniformBufferObject<DefaultTransformData>
	{
		public DefaultTransformBuffer()
		{
			ShadowStore = false;
		}

		public new DefaultTransformData Data;

		public void UpdateData()
		{
			Upload(new DefaultTransformData[] { Data });
		}

		public void UpdateData(DefaultTransformData data)
		{
			Data = data;
			UpdateData();
		}

		public void UpdatePosition(Vector3 position)
		{
			Data.Position = new Vector4(position, 1.0f);
			var data = new float[] {
				position.X, position.Y, position.Z, 1.0f,
			};
			UploadRangeRaw(data, 0, 4);
		}

		public void UpdateRotation(Quaternion rotation)
		{
			Data.Rotation = rotation;
			var data = new float[] {
				rotation.X, rotation.Y, rotation.Z, rotation.W,
			};
			UploadRangeRaw(data, 4, 4);
		}

		public void UpdateScale(Vector3 scale)
		{
			Data.Scale = new Vector4(scale, 0);
			var data = new float[] {
				scale.X, scale.Y, scale.Z, 0f
			};
			UploadRangeRaw(data, 8, 4);
		}

		public void UpdatePositionRotationScale(Vector4 position, Quaternion rotation, Vector4 scale)
		{
			Data.Position = position;
			Data.Rotation = rotation;
			Data.Scale = scale;
			var data = new float[] {
				position.X, position.Y, position.Z, 1.0f,
				rotation.X, rotation.Y, rotation.Z, rotation.W,
				scale.X, scale.Y, scale.Z, 0f
			};
			UploadRangeRaw(data, 0, 12);
		}

		public void UpdateViewProjection(Matrix4 viewProjection)
		{
			Data.ViewProjection = viewProjection;
			var data = new float[] {
				viewProjection.M11,
				viewProjection.M12,
				viewProjection.M13,
				viewProjection.M14,
				viewProjection.M21,
				viewProjection.M22,
				viewProjection.M23,
				viewProjection.M24,
				viewProjection.M31,
				viewProjection.M32,
				viewProjection.M33,
				viewProjection.M34,
				viewProjection.M41,
				viewProjection.M42,
				viewProjection.M43,
				viewProjection.M44,
			};
			UploadRangeRaw(data, 12, 16);
		}
	}
}
