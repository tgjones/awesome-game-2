sampler gTextureSampler1: register( s0 );
sampler gTextureSampler2: register( s1 );

float gWidth;
float gHeight;

float4 PixelShaderFunction( 
	float2 UV: TEXCOORD0,
	uniform sampler TextureSampler1 ) : COLOR0
{
	float OffsetX = 0.5 / gWidth;
	float OffsetY = 0.5 / gHeight;	
	float2 Coords = UV;
	float4 Color = tex2D( TextureSampler1, UV );
	
    return float4( Color.rgb, 1.0 );
}

technique Technique1
{
    pass p0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction(
			gTextureSampler1 );
    }
}
