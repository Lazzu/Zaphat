#version 410

layout(location = 0) in vec2 vPosition;
layout(location = 1) in vec4 TexCoord;
layout(location = 2) in vec4 Color;
layout(location = 3) in vec4 Rotation;
layout(location = 4) in vec4 Position;
layout(location = 5) in vec4 Scale;

out vec4 color;
out vec2 uv;

uniform mat4 ViewProjection;

vec3 rotate_vec3(vec3 v, vec4 q)
{
    return v + 2.0 * cross(q.xyz, cross(q.xyz, v) + q.w * v);
}

void main()
{
    vec3 vPos = vec3(vPosition - vec2(0.5, 0.5), 0.0);
    gl_Position = ViewProjection * (vec4(rotate_vec3(vPos * Scale.xyz, Rotation), 0.0) + Position);
    uv = TexCoord.xy + (vPosition * TexCoord.zw);
    color = Color;
}
