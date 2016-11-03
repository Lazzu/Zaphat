#version 420

layout(location = 0) in vec3 vertex;
layout(location = 1) in vec4 color;
layout(location = 2) in vec3 uv;
layout(location = 3) in vec3 normal;

out vec2 tcoord;
out vec4 fColor;
out vec3 fPosition;
out vec3 fNormal;

layout(std140) uniform ViewProjectionBlock
{
	mat4 View;
	mat4 Projection;
	mat4 InvView;
	mat4 ViewProjection;
	vec4 CameraWorldPosition;
	vec4 CameraWorldDirection;
} data_viewProjection;

layout(std140) uniform TransformBlock
{
	vec4 objPosition;
	vec4 objRotation;
	vec4 objScale;
} obj_transform;



uniform float time;

vec3 Rotate( vec4 q, vec3 v )
{ 
	return v + 2.0*cross(cross(v, q.xyz ) + q.w*v, q.xyz);
}

vec4 Transform(vec3 v)
{
	v = Rotate(obj_transform.objRotation, v);
	//v *= obj_transform.objScale.xyz;
	return vec4(v + obj_transform.objPosition.xyz, 1.0);
}

vec3 RotateNormal(vec3 n)
{
	return Rotate(obj_transform.objRotation, n);
}

void main()
{
	tcoord = uv.xy;
	fColor = vec4(1,1,1,1);
	//fColor = vec4(min(vec3(1.0), max(vec3(0.0), abs(Projection[gl_VertexID].xyz))), 1.0);
	//fColor = vec4(abs(CameraWorldPosition.xyz), 1.0);
	fColor = vec4(abs(obj_transform.objPosition.xyz), 1.0);
	//fColor = abs(vec4(data_viewProjection.View[0].x,0,0,1));
	//fColor = abs(vec4(data_viewProjection.View[0].x, data_viewProjection.View[0].y, data_viewProjection.View[0].z, 1.0));
	fNormal = RotateNormal(normal);
	//vec4 pos = Projection * View * Transform(vertex);
	//vec4 pos = Transform(vertex);
	vec4 pos = vec4(vertex * 0.5, 1.0);
	float invW = 1.0 / pos.w;
	fPosition = pos.xyz * invW;
	//fPosition = View[0].xyz;
	gl_Position = pos;
}

