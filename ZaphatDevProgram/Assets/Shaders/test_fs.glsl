#version 420
precision highp float;

in vec4 fColor;
in vec3 fPosition;
in vec3 fNormal;

layout(location = 0) out vec4 RGBA;

const float gamma = 1.0 / 0.5;
const float brightness = 1.25;

uniform vec3 lightPosition;

void main( void )
{
	vec3 lp = lightPosition - fPosition;
	vec3 lp2 = -lightPosition;
	vec3 lp3 = vec3(lp.y, -lp.x, lp.z);
	vec3 lp4 = -lp3;

    float att = (length(lp));
    float att2 = (length(lp2));
    float att3 = (length(lp3));
    float att4 = (length(lp4));

    vec3 ld = normalize(lp);
    vec3 ld2 = vec3(ld.y, -ld.x, ld.z);

    float light = pow( abs(dot(ld, fNormal)) * (att + att2), gamma);
    light += pow(abs(dot(ld2, fNormal)) * (att3 + att4), gamma);

	RGBA = vec4(light * fColor.rgb, fColor.a );








	RGBA = vec4(abs(fPosition.xyz), 1.0);




	//RGBA = vec4(light, light, light, 1.0);
	//RGBA = fColor;
	//RGBA = vec4(fPosition, 1.0);
	//RGBA = vec4(fNormal, 1.0);
	//RGBA = vec4(fPosition - lightPosition, 1.0);
}
