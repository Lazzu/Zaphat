using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using Zaphat.Application;
using Zaphat.Core;
using Zaphat.Rendering;
using Zaphat.Assets.Meshes;
using System.Collections.Generic;

namespace ZaphatDevProgram
{
	public class DevApp : ZapApp
	{
		Random r = new Random();

		ShaderProgram program;

		Shader vertex;
		Shader fragment;

		VertexArrayObject vao;
		ElementArrayBuffer<int> indices;
		ArrayBufferVector3 vertices;
		ArrayBufferVector3 normals;
		ArrayBufferVector4 colors;

		DefaultTransformBuffer Transform;
        DefaultViewProjectionBuffer ViewProjection;

		public DevApp(int width, int height, GraphicsMode mode) : base(width, height, mode)
		{
			VSync = VSyncMode.Adaptive;
		}

		protected override void OnLoad(EventArgs e)
		{
			//vao = new VertexArrayObject();

			//GL.ClearColor((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1.0f);
			GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
			GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

			vao = new VertexArrayObject();

			vao.Bind();

			indices = new ElementArrayBuffer<int>();

			var indexData = new int[] {
				0,1,2,3
			};

			indices.Bind();
			indices.Upload(indexData);

			vertices = new ArrayBufferVector3();

			var vertexData = new Vector3[] {
				new Vector3(-1.0f, -1.0f, 0.0f),
				new Vector3(1.0f, -1.0f, 0.0f),
				new Vector3(-1.0f, 1.0f, 0.0f),
				new Vector3(1.0f, 1.0f, 0.0f),

			};

			vertices.Bind();
			vertices.Upload(vertexData);
			vertices.BindVertexAttrib(0);

			normals = new ArrayBufferVector3();

			var normalData = new Vector3[] {
				new Vector3(-1.0f, -1.0f, -1.0f).Normalized(),
				new Vector3(1.0f, -1.0f, -1.0f).Normalized(),
				new Vector3(-1.0f, 1.0f, -1.0f).Normalized(),
				new Vector3(1.0f, 1.0f, -1.0f).Normalized(),

			};

			normals.Bind();
			normals.Upload(normalData);
			normals.BindVertexAttrib(3);

			colors = new ArrayBufferVector4();

			var colorData = new Vector4[] {
				new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
				new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
				new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
				new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
			};

			colors.Bind();
			colors.Upload(colorData);
			colors.BindVertexAttrib(1);

			vao.UnBind();

			program = new ShaderProgram();
			vertex = new Shader(ShaderType.VertexShader);
			fragment = new Shader(ShaderType.FragmentShader);

			vertex.ShaderSourceFile("Assets/Shaders/test_vs.glsl");
			fragment.ShaderSourceFile("Assets/Shaders/test_fs.glsl");

			program.AttachShader(vertex);
			program.AttachShader(fragment);

			program.Link();

			Transform = new DefaultTransformBuffer();

			program.BindUniformBlock("TransformBlock", Transform);

        	var projectionMatrix = Matrix4.Identity;
			var viewMatrix = Matrix4.Identity;

            var cameraPosition = new Vector3(0f, 0f, 0f);

            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)Width / (float)Height, 1.0f, 10.0f, out projectionMatrix);
            Matrix4.CreateTranslation(ref cameraPosition, out viewMatrix);

            Transform.Data = new DefaultTransformData()
			{
				Position = new Vector4(0f, 0f, 0f, 1f),
				Rotation = Quaternion.Identity,
				Scale = Vector4.One * 0.5f,
			};

			Transform.UpdateData();

            ViewProjection = new DefaultViewProjectionBuffer();
            ViewProjection.UpdateData();
            program.BindUniformBlock("ViewProjectionBlock", ViewProjection);
            //ViewProjection.Update(viewMatrix, projectionMatrix, cameraPosition, new Vector3(0, 0, 1f));
		}

		double totalTime = 0.0;

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			totalTime += e.Time;

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            program.Use();

            //ViewProjection.Update(Matrix4.Identity, Matrix4.Identity, Vector3.Zero, Vector3.One.Normalized());
            program.BindUniformBlock("ViewProjectionBlock", ViewProjection);

            //Transform.UpdateRotation(Quaternion.FromEulerAngles((float)-totalTime, 0.0f, 0.0f));
            program.BindUniformBlock("TransformBlock", Transform);

            var lightPosition = new Vector3((float)Math.Sin(totalTime), (float)Math.Cos(totalTime), 0.0f).Normalized();
			lightPosition *= (float)((Math.Sin(totalTime) + 1.0) * 0.5) + 0.25f;
			lightPosition *= (float)((Math.Sin(totalTime * 0.9) + 1.0) * 0.5) + 0.25f;
			lightPosition *= (float)((Math.Sin(totalTime * 0.8) + 1.0) * 0.5) + 0.25f;
			//lightPosition *= 2.75f;

			program.SendUniform("lightPosition", ref lightPosition);

			vao.Bind();
			GL.DrawElements(PrimitiveType.TriangleStrip, 4, DrawElementsType.UnsignedInt, IntPtr.Zero);

			SwapBuffers();
		}
	}
}
