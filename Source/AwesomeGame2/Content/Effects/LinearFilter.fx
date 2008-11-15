sampler gRenderTarget: register( s0 );

float2 gTextureSize;

float4 PixelShaderFunction( 
	float2 UV: TEXCOORD0,
	uniform float2 TextureSize ) : COLOR0
{
	float OffsetX = 1.0 / gTextureSize.x;
	float OffsetY = 1.0 / gTextureSize.y;	
	float2 Coords;
	float4 Color = tex2D( gRenderTarget, UV );

	Coords.x = UV.x + OffsetX;
	Coords.y = UV.y;
	Color += tex2D( gRenderTarget, Coords );
	Coords.x = UV.x - OffsetX;
	Coords.y = UV.y;
	Color += tex2D( gRenderTarget, Coords );
	Coords.x = UV.x;
	Coords.y = UV.y + OffsetY;
	Color += tex2D( gRenderTarget, Coords );
	Coords.x = UV.x;
	Coords.y = UV.y - OffsetY;
	Color += tex2D( gRenderTarget, Coords );
	Color *= 0.2;
	
    return float4( Color.rgb, 1.0 );
}

technique Technique1
{
    pass p0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction(
			gTextureSize );
    }
}
