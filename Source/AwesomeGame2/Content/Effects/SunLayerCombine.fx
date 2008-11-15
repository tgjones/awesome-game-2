sampler gInputSampler : register( s0 );

texture2D gGradientTexture;

sampler gGradientSampler = sampler_state
{
	Texture = <gGradientTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap; 
};

float gfThreshold;
float gfBrightness;

float Luminance( float4 Color )
{
	return 
		0.3 * Color.r +
		0.6 * Color.g +
		0.1 * Color.b;
}

float4 PS_ExtractHDR( 
	float2 inTex: TEXCOORD0,
	uniform float Threshold,
	uniform float Brightness ) : COLOR0
{	
	float4 color = tex2D( gInputSampler, inTex );
	float lum = Luminance( color );
	if( lum >= Threshold )
	{
		float pos = lum - Threshold;
		if( pos > 0.98 ) pos = 0.98;
		if( pos < 0.02 ) pos = 0.02;
		color = tex2D( gGradientSampler, pos ) * Brightness;
		return float4( color.rgb, 1.0 );
	}
	else
	{
		return float4( 0.0, 0.0, 0.0, 1.0 );
	}
} 

technique Main
{
    pass p0
	{		
		PixelShader = compile ps_2_0 PS_ExtractHDR(
			gfThreshold,
			gfBrightness );	
    }
}