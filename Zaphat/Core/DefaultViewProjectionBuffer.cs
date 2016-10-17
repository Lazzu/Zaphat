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
            ShadowStore = false;
            Data = new DefaultViewProjectionData();
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
            Data.View = view;
            Data.Projection = projection;
            Data.ViewProjection = projection * view;
            //Data.InvProjection = projection.Inverted();
            Data.InvView = view.Inverted();
            Data.CameraWorldPosition = new Vector4(cameraWorldPos, 1.0f);
            Data.CameraWorldDirection = new Vector4(cameraWorldDirection, 0.0f);
            UpdateData();
        }
    }
}
