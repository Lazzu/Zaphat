using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zaphat.Core;

namespace Zaphat.Assets.Textures
{
    public class SubTexture : Texture
    {
        public Box2 Region;

        public Texture Parent { get; protected set; }

        public SubTexture(Texture texture) : base(texture.Target)
        {
            GLName = texture.GLName;
            Settings = texture.Settings;
            Path = texture.Path;

            Parent = texture;
            Region = new Box2(0f, 0f, 1f, 1f);
        }

        public void Dispose()
        {
            Parent.RemoveSubTexture(this);
            Parent = null;
        }
    }
}