using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using Zaphat.Application;
using Zaphat.Core;
using Zaphat.Rendering;
using Zaphat.Assets.Textures;
using Zaphat.Utilities;
using Zaphat.Text;

namespace ZaphatDevProgram
{
	public class DevApp : ZapApp
	{
		Random _r = new Random();

		ShaderProgram _program;

		Shader _vertex;
		Shader _fragment;

		VertexArrayObject _vao;
		ElementArrayBuffer<int> _indices;
		ArrayBufferVector3 _vertices;
		ArrayBufferVector3 _normals;
		ArrayBufferVector4 _colors;
		ArrayBufferVector2 _tcoords;

		DefaultTransformBuffer _transform;
		DefaultViewProjectionBuffer _viewProjection;

		Matrix4 _projectionMatrix = Matrix4.Identity;
		Matrix4 _viewMatrix = Matrix4.Identity;
		Vector3 _cameraPosition;
		Vector3 _cameraDirection = new Vector3(0, 0, 1f);

		TextureManager _textureManager;
		Texture _diffuseTexture;
		Texture _normalTexture;
		Texture _specularTexture;

		TextMesh _text;

		public DevApp(int width, int height, GraphicsMode mode) : base(width, height, mode)
		{
			Console.WriteLine("Foobar!");
			VSync = VSyncMode.On;
		}

		protected override void OnLoad(EventArgs e)
		{
			Logger.Log("Start loading mandatory assets.");
			//vao = new VertexArrayObject();

			//GL.ClearColor((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1.0f);
			GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
			GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

			_vao = new VertexArrayObject();

			_vao.Bind();

			_indices = new ElementArrayBuffer<int>();

			var indexData = new int[36];
			for (var i = 0; i < indexData.Length; i++)
			{
				indexData[i] = i;
			}

			_indices.Bind();
			_indices.Upload(indexData);

			_vertices = new ArrayBufferVector3();

			var vertexData = new[] {
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

			_vertices.Bind();
			_vertices.Upload(vertexData);
			_vertices.BindVertexAttrib(0);

			_normals = new ArrayBufferVector3();

			var normal = new Vector3(1, 1, 1).Normalized();

			var normalData = new Vector3[vertexData.Length];
			for (var i = 0; i < normalData.Length; i++)
			{
				normalData[i] = vertexData[i] * normal;
			}

			_normals.Bind();
			_normals.Upload(normalData);
			_normals.BindVertexAttrib(3);

			_tcoords = new ArrayBufferVector2();

			var tcoordData = new[]
			{
				new Vector2(0f, 1.0f),
				new Vector2(1.0f, 0f),
				new Vector2(0f, 0.0f),
				new Vector2(0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f),

				new Vector2(0.0f, 0.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(1.0f, 1.0f),

				new Vector2(0.0f, 1.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(1.0f, 1.0f),

				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f),

				new Vector2(0.0f, 0.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(1.0f, 1.0f),

				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
			};

			_tcoords.Bind();
			_tcoords.Upload(tcoordData);
			_tcoords.BindVertexAttrib(2);

			_colors = new ArrayBufferVector4();

			var colorData = new Vector4[vertexData.Length];
			for (var i = 0; i < colorData.Length; i++)
			{
				colorData[i] = Vector4.One;
			}

			_colors.Bind();
			_colors.Upload(colorData);
			_colors.BindVertexAttrib(1);



			_vao.UnBind();

			_program = new ShaderProgram("Some shader");
			_vertex = new Shader(ShaderType.VertexShader);
			_fragment = new Shader(ShaderType.FragmentShader);

			_vertex.ShaderSourceFile("Assets/Shaders/test_vs.glsl");
			_fragment.ShaderSourceFile("Assets/Shaders/test_fs.glsl");

			_program.AttachShader(_vertex);
			_program.AttachShader(_fragment);

			_program.Link();

			System.Diagnostics.Debug.WriteLine("Create Transform buffer");
		    _transform = new DefaultTransformBuffer {BindingPoint = 1};
		    _program.BindUniformBlock("TransformBlock", _transform);

			Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)Width / (float)Height, 1.0f, 100.0f, out _projectionMatrix);
			Matrix4.CreateTranslation(ref _cameraPosition, out _viewMatrix);

			_transform.Data = new DefaultTransformData()
			{
				Position = new Vector4(0f, 0f, 0f, 1f),
				Rotation = Quaternion.Identity,
				Scale = Vector4.One * 0.5f,
			};

			_transform.UpdateData();

		    _viewProjection = new DefaultViewProjectionBuffer {BindingPoint = 2};
		    _program.BindUniformBlock("ViewProjectionBlock", _viewProjection);
			_viewProjection.Update(_viewMatrix, _projectionMatrix, _cameraPosition, _cameraDirection);

			Logger.CheckGLError();

			_textureManager = TextureManager.Global;

			_diffuseTexture = _textureManager.Load("Assets/Fonts/font3.png");
			_normalTexture = _textureManager.Load("Assets/Textures/pattern_133_normal.png");
			_specularTexture = _textureManager.Load("Assets/Textures/pattern_133_specular.png");

            Logger.CheckGLError();

            _diffuseTexture.Settings = new TextureSettings(TextureWrappingMode.Repeat, TextureFilterMode.Trilinear, 16f, 1);
			_diffuseTexture.ApplySettings();

            Logger.CheckGLError();

            _program.BindTextureUnit(_diffuseTexture, "DiffuseTexture", 0);
			_program.BindTextureUnit(_normalTexture, "NormalTexture", 1);
			_program.BindTextureUnit(_specularTexture, "SpecularTexture", 2);

			var font = Font.Load("Assets/Fonts/font3.fnt");

			var textProgram = new ShaderProgram("SDF");
			var textVertex = new Shader(ShaderType.VertexShader);
			var textFragment = new Shader(ShaderType.FragmentShader);

			textVertex.ShaderSourceFile("Assets/Fonts/sdf_vs.glsl");
			textFragment.ShaderSourceFile("Assets/Fonts/sdf_fs.glsl");

			textProgram.AttachShader(textVertex);
			textProgram.AttachShader(textFragment);

			textProgram.Link();

		    _text = new TextMesh
		    {
		        ShaderProgram = textProgram,
		        Font = font,
		        Text =
		            "!\"#$%&'()*+,-./0123456789:;<=>?@\nABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`\nabcdefghijklmnopqrstuvwxyz{|}~\u007f€‚ƒ„…†‡ˆ‰Š‹ŒŽ\n‘’“”•–—˜™š›œžŸ ¡¢£¤¥¦§¨©ª«¬­®¯°\n±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×\nØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ"
		    };
		    //text.Text = "mutsis";
			//text.Text = "Testaan toimiiko tämä teksti.";

			Logger.Log("Loaded mandatory assets, now able to start running!");
		}

		protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Key == OpenTK.Input.Key.A)
			{
				/*if (diffuseTexture.Settings.AnisotrophyLevel > 1)
				{
					diffuseTexture.Settings.AnisotrophyLevel = 0;
				}
				else
				{
					diffuseTexture.Settings.AnisotrophyLevel = 16;
				}
				diffuseTexture.ApplySettings();*/
			}
		}

		double _totalTime = 0.0;

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			_totalTime += e.Time;

			GL.Viewport(0, 0, Width, Height);
			GL.Enable(EnableCap.DepthTest);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



			_program.Use();

			_program.BindTextureUnit(_diffuseTexture, "DiffuseTexture", 0);
			_program.BindTextureUnit(_normalTexture, "NormalTexture", 1);
			_program.BindTextureUnit(_diffuseTexture, "SpecularTexture", 2);

			_cameraPosition = new Vector3((float)Math.Sin(_totalTime * 0.25) * 50 + (float)Math.Sin(_totalTime * 0.53 * 0.25) * 10, (float)Math.Sin(_totalTime * 0.912345 * 0.25) * 25, (float)Math.Cos(_totalTime * 0.25) * 50 + (float)Math.Cos(_totalTime * 0.5357 * 0.25) * 10);

			Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, ((float)Width) / ((float)Height), 15f, 75.0f, out _projectionMatrix);
			_viewMatrix = Matrix4.LookAt(_cameraPosition, Vector3.Zero, Vector3.UnitY);
			var cameraDir = new Vector4(0, 0, 1, 0) * _viewMatrix;

			_viewProjection.Update(_viewMatrix, _projectionMatrix, _cameraPosition, new Vector3(cameraDir.X, cameraDir.Y, cameraDir.Z));
			//var rot = Quaternion.FromEulerAngles(0.0f, (float)totalTime, 0.0f);
			_transform.UpdatePositionRotationScale(new Vector4(0f, 0f, 0f, 1f), Quaternion.Identity, Vector4.One * 10f);

			var lightposition = new Vector3((float)Math.Sin(_totalTime), 1.0f, (float)Math.Cos(_totalTime));
			_program.SendUniform("LightPosition", lightposition);

			_vao.Bind();
			GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			_text.Draw();

			SwapBuffers();

			Zaphat.Utilities.Logger.CheckGLError();
		}


	}
}
