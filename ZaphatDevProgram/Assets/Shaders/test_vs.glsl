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
	vec4 objPosition;
	vec4 objRotation;
	vec4 objScale;
};

layout(std140) uniform ViewProjectionBlock
{
    mat4 ViewProjection;
	mat4 View;
	mat4 Projection;
	mat4 InvView;
	vec4 CameraWorldPosition;
	vec4 CameraWorldDirection;
};

uniform float time;

vec3 Rotate( vec4 q, vec3 v )
{ 
	return v + 2.0*cross(cross(v, q.xyz ) + q.w*v, q.xyz);
}

vec4 Transform(vec3 v)
{
	v = Rotate(objRotation, v);
	v *= objScale.xyz;
	return vec4(v + objPosition.xyz, 1.0);
}

vec3 RotateNormal(vec3 n)
{
	return Rotate(objRotation, n);
}

void main()
{
	tcoord = uv.xy;
	fColor = color;
	fNormal = RotateNormal(normal);
	//vec4 pos = Transform(vertex);
	vec4 pos = ViewProjection * Transform(vertex);
	float invW = 1.0 / pos.w;
	fPosition = pos.xyz * invW;
	//fPosition = foobar.xyz;
	gl_Position = pos;
}

