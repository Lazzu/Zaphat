#version 410

in vec4 color;
in vec2 uv;

uniform sampler2D spriteTexture;

out vec4 fragColor;

void main()
{
	vec4 texel = texture2D(spriteTexture, uv);
    fragColor = color * texel;
}