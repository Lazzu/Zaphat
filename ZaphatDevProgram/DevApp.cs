using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using Zaphat.Application;
using Zaphat.Rendering;
using Zaphat;

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

		public DevApp(int width, int height, GraphicsMode mode) : base(width, height, mode)
		{

		}

		protected override void OnLoad(EventArgs e)
		{
			//vao = new VertexArrayObject();

			//GL.ClearColor((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1.0f);
			GL.ClearColor(0.9f, 0.9f, 0.9f, 1.0f);

			vao = new VertexArrayObject();

			vao.Bind();

			indices = new ElementArrayBuffer<int>();

			indices.Data = new int[] {
				0,1,2
			};

			indices.Bind();
			indices.Upload();

			vertices = new ArrayBufferVector3();

			vertices.Data = new Vector3[] {
				new Vector3(-1.0f, -1.0f, 0.0f),
				new Vector3(1.0f, -1.0f, 0.0f),
				new Vector3(0.0f, 1.0f, 0.0f),
			};

			vertices.Bind();
			vertices.Upload();
			vertices.BindVertexAttrib(0);

			normals = new ArrayBufferVector3();

			normals.Data = new Vector3[] {
				new Vector3(-1.0f, -1.0f, -1.0f).Normalized(),
				new Vector3(1.0f, -1.0f, -1.0f).Normalized(),
				new Vector3(0.0f, 1.0f, -1.0f).Normalized(),
			};

			normals.Bind();
			normals.Upload();
			normals.BindVertexAttrib(3);

			colors = new ArrayBufferVector4();

			colors.Data = new Vector4[] {
				new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
				new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
				new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
			};

			colors.Bind();
			colors.Upload();
			colors.BindVertexAttrib(1);

			vao.UnBind();

			program = new ShaderProgram();
			vertex = new Shader(ShaderType.VertexShader);
			fragment = new Shader(ShaderType.FragmentShader);

			vertex.ShaderSourceFile("Assets/Shaders/test.vs");
			fragment.ShaderSourceFile("Assets/Shaders/test.fs");

			program.AttachShader(vertex);
			program.AttachShader(fragment);

			program.Link();
		}

		double totalTime = 0.0;

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			totalTime += e.Time;

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			program.Use();

			var projectionMatrix = Matrix4.Identity;
			var viewMatrix = Matrix4.Identity;
			var viewProjectionMatrix = projectionMatrix * viewMatrix;
			var modelMatrix = Matrix4.Identity;
			var modelViewMatrix = viewMatrix * modelMatrix;
			var normalMatrix = Matrix4.Transpose(Matrix4.Invert(modelViewMatrix));

			program.SendUniform("mViewProjection", ref viewProjectionMatrix);
			program.SendUniform("mModel", ref modelMatrix);
			program.SendUniform("mNormal", ref normalMatrix);

			var lightPosition = new Vector3((float)Math.Sin(totalTime), (float)Math.Cos(totalTime), 0.0f).Normalized();

			program.SendUniform("lightPosition", ref lightPosition);

			vao.Bind();
			GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, IntPtr.Zero);

			SwapBuffers();
		}
	}
}
