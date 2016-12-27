#version 410

layout(location = 0) in vec2 position;
layout(location = 1) in vec2 texcoord;

out vec2 uv;

uniform float scale = 1;


void main()
{
    gl_Position = vec4(scale * vec3(position, 0.0), 1.0);
    uv = texcoord;
}
