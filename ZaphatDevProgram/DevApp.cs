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
using Zaphat.Sprites;

namespace ZaphatDevProgram
{
    public class DevApp : ZapApp
    {
		
        TextMesh _text;
        Sprite _sprite;

        public DevApp(int width, int height, GraphicsMode mode)
            : base(width, height, mode)
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

            var font = Font.Load("Assets/Fonts/LiberationSans.fnt");

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
                Text = "Foobar"
            };

            var spriteShader = new ShaderProgram("SpriteShader");
            var spriteVertexShader = new Shader(ShaderType.VertexShader);
            var spriteFragmentShader = new Shader(ShaderType.FragmentShader);

            spriteVertexShader.ShaderSourceFile("Assets/Shaders/sprite_vs.glsl");
            spriteFragmentShader.ShaderSourceFile("Assets/Shaders/sprite_fs.glsl");

            spriteShader.AttachShader(spriteVertexShader);
            spriteShader.AttachShader(spriteFragmentShader);

            spriteShader.Link();

            _sprite = new Sprite();
            _sprite.Name = "Ship";
            _sprite.Shader = spriteShader;

            Logger.Log("Loaded mandatory assets, now able to start running!");
        }

        protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == OpenTK.Input.Key.A)
            {

            }
        }

        double _totalTime = 0.0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _totalTime += e.Time;

            GL.Viewport(0, 0, Width, Height);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            _text.Draw();

            SwapBuffers();

            Zaphat.Utilities.Logger.CheckGLError();
        }


    }
}
