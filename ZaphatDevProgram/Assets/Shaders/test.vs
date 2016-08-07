#version 330

layout(location = 0) in vec3 vertex;
layout(location = 1) in vec4 color;
layout(location = 2) in vec3 uv;
layout(location = 3) in vec3 normal;

out vec2 tcoord;
out vec4 fColor;
out vec3 fPosition;
out vec3 fNormal;

uniform mat4 mViewProjection;
uniform mat4 mModel;
uniform mat4 mNormal;
uniform float time;

void main()
{
	tcoord = uv.xy;
	fColor = color;
	fNormal = (mNormal * vec4(normal, 0.0)).xyz;
	vec4 pos = mViewProjection * mModel * vec4(vertex , 1.0);
	fPosition = pos.xyz / pos.w;
	gl_Position = pos;
}

