#version 330

in vec4 fColor;
in vec3 fPosition;
in vec3 fNormal;

layout(location = 0) out vec4 RGBA;

uniform vec3 lightPosition;

void main( void )
{
    float light = pow(max(0.0, min(1.0, dot(normalize(lightPosition), fNormal))), 1.0/2.2);
	RGBA = vec4(light * fColor.rgb, fColor.a );
	//RGBA = vec4(light, light, light, 1.0);
	//RGBA = fColor;
	//RGBA = vec4(fPosition, 1.0);
	//RGBA = vec4(-fNormal, 1.0);
	//RGBA = vec4(lightPosition, 1.0);
}
