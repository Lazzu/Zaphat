#version 410

layout(location = 0) in vec2 position;
layout(location = 1) in vec2 texcoord;

out vec2 uv;

uniform vec2 Scale;
uniform vec2 PosOnScreen = vec2( -0.75, 0.25 );
uniform int Frame;

void main()
{
	float size = (-sin(Frame * 0.001) + 1.1) * 3;
    gl_Position = vec4(vec3(((Scale * position) + PosOnScreen) * vec2(size, size), 0.0), 1.0);
    uv = texcoord;
}
