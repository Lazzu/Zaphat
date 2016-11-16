using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using Zaphat.Application;
using Zaphat.Core;
using Zaphat.Rendering;
using Zaphat.Assets.Textures;

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
		ArrayBufferVector2 tcoords;

		DefaultTransformBuffer Transform;
		DefaultViewProjectionBuffer ViewProjection;

		Matrix4 projectionMatrix = Matrix4.Identity;
		Matrix4 viewMatrix = Matrix4.Identity;
		Vector3 CameraPosition;
		Vector3 CameraDirection = new Vector3(0, 0, 1f);

		TextureManager textureManager;
		Texture diffuseTexture;
		Texture normalTexture;
		Texture specularTexture;

		public DevApp(int width, int height, GraphicsMode mode) : base(width, height, mode)
		{
			VSync = VSyncMode.On;
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

			var indexData = new int[36];
			for (int i = 0; i < indexData.Length; i++)
			{
				indexData[i] = i;
			}

			indices.Bind();
			indices.Upload(indexData);

			vertices = new ArrayBufferVector3();

			var vertexData = new Vector3[] {
				new Vector3(-1.0f, -1.0f, 1.0f),
				new Vector3(1.0f, -1.0f, -1.0f),
				new Vector3(-1.0f, -1.0f, -1.0f),
				new Vector3(-1.0f, -1.0f, 1.0f),
				new Vector3(1.0f, -1.0f, 1.0f),
				new Vector3(1.0f, -1.0f, -1.0f),
				new Vector3(-1.0f, 1.0f, -1.0f),
				new Vector3(1.0f, 1.0f, -1.0f),
				new Vector3(-1.0f, 1.0f, 1.0f),
				new Vector3(-1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, -1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(-1.0f, -1.0f, 1.0f),
				new Vector3(-1.0f, -1.0f, -1.0f),
				new Vector3(-1.0f, 1.0f, -1.0f),
				new Vector3(-1.0f, -1.0f, 1.0f),
				new Vector3(-1.0f, 1.0f, -1.0f),
				new Vector3(-1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, -1.0f, -1.0f),
				new Vector3(1.0f, -1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, -1.0f),
				new Vector3(1.0f, -1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, -1.0f),
				new Vector3(-1.0f, -1.0f, -1.0f),
				new Vector3(1.0f, -1.0f, -1.0f),
				new Vector3(-1.0f, 1.0f, -1.0f),
				new Vector3(-1.0f, 1.0f, -1.0f),
				new Vector3(1.0f, -1.0f, -1.0f),
				new Vector3(1.0f, 1.0f, -1.0f),
				new Vector3(1.0f, -1.0f, 1.0f),
				new Vector3(-1.0f, -1.0f, 1.0f),
				new Vector3(-1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, -1.0f, 1.0f),
				new Vector3(-1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
			};

			vertices.Bind();
			vertices.Upload(vertexData);
			vertices.BindVertexAttrib(0);

			normals = new ArrayBufferVector3();

			var normal = new Vector3(1, 1, 1).Normalized();

			var normalData = new Vector3[vertexData.Length];
			for (int i = 0; i < normalData.Length; i++)
			{
				normalData[i] = vertexData[i] * normal;
			}

			normals.Bind();
			normals.Upload(normalData);
			normals.BindVertexAttrib(3);

			tcoords = new ArrayBufferVector2();

			var tcoordData = new Vector2[vertexData.Length];
			for (int i = 0; i < 6; i++)
			{
				tcoordData[i * 6 + 0] = new Vector2(0, 1);
				tcoordData[i * 6 + 1] = new Vector2(1, 0);
				tcoordData[i * 6 + 2] = new Vector2(0, 0);

				tcoordData[i * 6 + 3] = new Vector2(0, 1);
				tcoordData[i * 6 + 4] = new Vector2(1, 1);
				tcoordData[i * 6 + 5] = new Vector2(1, 0);
			}

			tcoords.Bind();
			tcoords.Upload(tcoordData);
			tcoords.BindVertexAttrib(2);

			colors = new ArrayBufferVector4();

			var colorData = new Vector4[vertexData.Length];
			for (int i = 0; i < colorData.Length; i++)
			{
				colorData[i] = Vector4.One;
			}

			colors.Bind();
			colors.Upload(colorData);
			colors.BindVertexAttrib(1);



			vao.UnBind();

			program = new ShaderProgram("Some shader");
			vertex = new Shader(ShaderType.VertexShader);
			fragment = new Shader(ShaderType.FragmentShader);

			vertex.ShaderSourceFile("Assets/Shaders/test_vs.glsl");
			fragment.ShaderSourceFile("Assets/Shaders/test_fs.glsl");

			program.AttachShader(vertex);
			program.AttachShader(fragment);

			program.Link();

			System.Diagnostics.Debug.WriteLine("Create Transform buffer");
			Transform = new DefaultTransformBuffer();
			Transform.BindingPoint = 1;
			program.BindUniformBlock("TransformBlock", Transform);

			Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)Width / (float)Height, 1.0f, 100.0f, out projectionMatrix);
			Matrix4.CreateTranslation(ref CameraPosition, out viewMatrix);

			Transform.Data = new DefaultTransformData()
			{
				Position = new Vector4(0f, 0f, 0f, 1f),
				Rotation = Quaternion.Identity,
				Scale = Vector4.One * 0.5f,
			};

			Transform.UpdateData();

			ViewProjection = new DefaultViewProjectionBuffer();
			ViewProjection.BindingPoint = 2;
			program.BindUniformBlock("ViewProjectionBlock", ViewProjection);
			ViewProjection.Update(viewMatrix, projectionMatrix, CameraPosition, CameraDirection);

			Zaphat.Utilities.Logger.CheckGLError();

			textureManager = new TextureManager();

			diffuseTexture = textureManager.Load("Assets/Textures/pattern_133_diffuse.png");
			normalTexture = textureManager.Load("Assets/Textures/pattern_133_normal.png");
			specularTexture = textureManager.Load("Assets/Textures/pattern_133_specular.png");

			program.BindTextureUnit(diffuseTexture, "DiffuseTexture", 0);
			program.BindTextureUnit(normalTexture, "NormalTexture", 1);
			program.BindTextureUnit(specularTexture, "SpecularTexture", 2);
		}

		double totalTime = 0.0;

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			totalTime += e.Time;

			GL.Viewport(0, 0, Width, Height);
			GL.Enable(EnableCap.DepthTest);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			program.Use();

			CameraPosition = new Vector3((float)Math.Sin(totalTime * 0.25) * 50 + (float)Math.Sin(totalTime * 0.53 * 0.25) * 10, (float)Math.Sin(totalTime * 0.912345 * 0.25) * 25, (float)Math.Cos(totalTime * 0.25) * 50 + (float)Math.Cos(totalTime * 0.5357 * 0.25) * 10);

			Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, ((float)Width) / ((float)Height), 15f, 75.0f, out projectionMatrix);
			viewMatrix = Matrix4.LookAt(CameraPosition, Vector3.Zero, Vector3.UnitY);
			var cameraDir = new Vector4(0, 0, 1, 0) * viewMatrix;

			ViewProjection.Update(viewMatrix, projectionMatrix, CameraPosition, new Vector3(cameraDir.X, cameraDir.Y, cameraDir.Z));
			//var rot = Quaternion.FromEulerAngles(0.0f, (float)totalTime, 0.0f);
			Transform.UpdatePositionRotationScale(new Vector4(0f, 0f, 0f, 1f), Quaternion.Identity, Vector4.One * 10f);

			var lightposition = new Vector3((float)Math.Sin(totalTime), 1.0f, (float)Math.Cos(totalTime));
			program.SendUniform("LightPosition", lightposition);

			vao.Bind();
			GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, IntPtr.Zero);

			SwapBuffers();

			Zaphat.Utilities.Logger.CheckGLError();
		}


	}
}
