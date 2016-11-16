#version 420

precision highp float;

in vec4 fColor;
in vec3 fPosition;
in vec3 fNormal;
in vec2 tcoord;

layout(location = 0) out vec4 RGBA;

uniform vec3 LightPosition = vec3(1,1,1);

uniform float MinLight = 0.5;
uniform float MaxLight = 0.95;

uniform sampler2D DiffuseTexture;
uniform sampler2D NormalTexture;
uniform sampler2D SpecularTexture;

float remap(float value, float fromLow, float fromHigh, float toLow, float toHigh)
{
	return toLow + (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow);
}

void main( void )
{
	vec3 lightDirection = normalize(LightPosition);
	vec3 halfVector = normalize(lightDirection + normalize(-fPosition));

	// Very basic lighting
	float nDotL = max(0.0, min(1.0, dot(normalize(fNormal), lightDirection)));
	float nDotH = max(0.0, min(1.0, dot(normalize(fNormal), normalize(halfVector))));

	float light = remap(nDotL, 0.0, 1.0, MinLight, MaxLight);
	float specularLight = pow(nDotH, 50.0);

    RGBA = vec4(light * texture(DiffuseTexture, tcoord.xy).rgb + specularLight * texture(SpecularTexture, tcoord.xy).rgb, 1.0);
    //RGBA = texture(DiffuseTexture, tcoord.xy).rgba;
	//RGBA = vec4(tcoord.x, tcoord.y, 0.0, 1.0);
}
