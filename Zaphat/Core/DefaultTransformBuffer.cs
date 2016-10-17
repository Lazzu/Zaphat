using OpenTK;
namespace Zaphat.Core
{
	public class DefaultTransformBuffer : UniformBufferObject<DefaultTransformData>
	{
        int floatSize;

		public DefaultTransformBuffer()
		{
            floatSize = sizeof(float);
			ShadowStore = false;
		}

		public new DefaultTransformData Data;

		public void UpdateData()
		{
			Upload(ref Data);
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
			UploadRangeRaw(data, 0, 4 * floatSize);
		}

		public void UpdateRotation(Quaternion rotation)
		{
			Data.Rotation = rotation;
			var data = new float[] {
				rotation.X, rotation.Y, rotation.Z, rotation.W,
			};
            Bind();
			UploadRangeRaw(data, 4 * floatSize, 4 * floatSize);
		}

		public void UpdateScale(Vector3 scale)
		{
			Data.Scale = new Vector4(scale, 0);
			var data = new float[] {
				scale.X, scale.Y, scale.Z, 0f
			};
			UploadRangeRaw(data, 8 * floatSize, 4 * floatSize);
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
			UploadRangeRaw(data, 0, 12 * floatSize);
		}
	}
}
