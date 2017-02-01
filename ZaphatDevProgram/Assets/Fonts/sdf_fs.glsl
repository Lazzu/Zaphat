#version 410


//#define ANTIALIAS 1

in vec2 uv;

uniform sampler2D sdfTexture;
uniform vec3 textColor = vec3(1.0, 1.0, 0.0);
uniform vec3 borderColor = vec3(1.0, 0.0, 0.0);
uniform float startDistance = 0.35;
uniform float borderWidth = 0.15;
uniform float borderStart = 0.45;

uniform vec2 Scale;
uniform float TextureHeight;

out vec4 fragColor;

void main()
{
	vec4 texel = texture2D(sdfTexture, Scale * vec2(uv.x, uv.y));
    float dist = 1.0 - texel.r;
#if ANTIALIAS
    float d  = (dist - 0.5); // distance rebias 0..1 --> -0.5 .. +0.5
    float aa = (startDistance + borderStart + borderWidth) * length( vec2( dFdx( d ), dFdy( d ) ) ); // anti-alias
    float alpha = 1.0 - smoothstep( -aa, aa, d );
#else
    float alpha = 1.0 - smoothstep( startDistance, startDistance + borderWidth, dist );
#endif

	if(alpha <= 0.001)
		discard;

	vec3 color = mix( textColor, borderColor, 1.0 - smoothstep( startDistance, borderStart, 1.0 - dist ) );

    fragColor = vec4(color, alpha);

    //fragColor =vec4(vec3(1.0-dist), 1.0);
    //fragColor = vec4(vec3(alpha), 1.0);
    //fragColor = vec4(vec3(scale * uv, 0.0), 1.0);
    //fragColor = texture2D(sdfTexture, Scale * vec2(uv.x, uv.y));
}
