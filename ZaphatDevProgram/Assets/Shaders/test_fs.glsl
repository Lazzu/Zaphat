#version 420
precision highp float;

in vec4 fColor;
in vec3 fPosition;
in vec3 fNormal;

layout(location = 0) out vec4 RGBA;

const vec3 lightDirection = vec3(1,1,1);

uniform float MinLight = 0.5;
uniform float MaxLight = 0.95;

float remap(float value, float fromLow, float fromHigh, float toLow, float toHigh)
{
	return toLow + (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow);
}

void main( void )
{
	// Very basic lighting
	float nDotL = max(0.0, min(1.0, dot(normalize(fNormal), normalize(lightDirection))));

	vec3 light = vec3(remap(nDotL, 0.0, 1.0, MinLight, MaxLight));

    RGBA = vec4(light * fColor.rgb, 1.0);
}
