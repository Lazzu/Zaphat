#version 410

//#define ANTIALIAS 1

in vec2 uv;

uniform sampler2D sdfTexture;
uniform vec3 textColor = vec3(1.0, 1.0, 0.0);
uniform float startDistance = 0.45f;
uniform float borderWidth = 0.05f;

out vec4 fragColor;

void main()
{
    float dist = 1.0 - texture2D(sdfTexture, uv).r;
#if ANTIALIAS
    float d  = (dist - 0.5); // distance rebias 0..1 --> -0.5 .. +0.5
    float aa = (startDistance + borderWidth)*length( vec2( dFdx( d ), dFdy( d ))); // anti-alias
    float alpha = smoothstep( -aa, aa, d );
#else
    float alpha = smoothstep( startDistance, startDistance + borderWidth, dist );
#endif

    fragColor = vec4(textColor, alpha);
    //fragColor =vec4(vec3(dist), 1.0);
    //fragColor = vec4(vec3(alpha), 1.0);
}
