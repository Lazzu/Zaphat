#version 420

layout(location = 0) in vec3 vertex;
layout(location = 1) in vec4 color;
layout(location = 2) in vec3 uv;
layout(location = 3) in vec3 normal;

out vec2 tcoord;
out vec4 fColor;
out vec3 fPosition;
out vec3 fNormal;

layout(std140) uniform TransformBlock
{
	vec4 Position;
	vec4 Rotation;
	vec3 Scale;
	mat4 mViewProjection;
} transform;

uniform float time;

vec3 Rotate( vec4 q, vec3 v )
{ 
	return v + 2.0*cross(cross(v, q.xyz ) + q.w*v, q.xyz);
}

vec4 Transform(vec3 v)
{
	v = Rotate(transform.Rotation, v);
	v *= transform.Scale;
	return vec4(v + transform.Position.xyz, 1.0);
}

vec3 RotateNormal(vec3 n)
{
	return Rotate(transform.Rotation, n);
}

void main()
{
	tcoord = uv.xy;
	fColor = color;
	fNormal = RotateNormal(normal);
	vec4 pos = transform.mViewProjection * Transform(vertex);
	fPosition = pos.xyz / pos.w;
	gl_Position = pos;
}

