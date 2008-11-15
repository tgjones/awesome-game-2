#include "SpriteBatchVertexShader.fxh"

float gScreenWidth;
float gScreenHeight;
float gScaleFactor = 1.5;

float gWeights7x7[7][7] = {
	{ 0.00000067, 0.00002292, 0.00019117, 
	  0.00038771, 0.00019117, 0.00002292, 0.00000067 }, 
	{ 0.00002292, 0.00078633, 0.00655965, 
	  0.01330373, 0.00655965, 0.00078633, 0.00002292 },
	{ 0.00019117, 0.00655965, 0.05472157, 
	  0.11098164, 0.05472157, 0.00655965, 0.00019117 },
	{ 0.00038771, 0.01330373, 0.11098164, 
	  0.22508352, 0.11098164, 0.01330373, 0.00038771 },
	{ 0.00019117, 0.00655965, 0.05472157, 
	  0.11098164, 0.05472157, 0.00655965, 0.00019117 },
	{ 0.00002292, 0.00078633, 0.00655965, 
	  0.01330373, 0.00655965, 0.00078633, 0.00002292 },
	{ 0.00000067, 0.00002292, 0.00019117, 
	  0.00038771, 0.00019117, 0.00002292, 0.00000067 }
	};

float gWeights5x5[5][5] = {
		{ 0.003, 0.013, 0.022, 0.013, 0.003 },
		{ 0.013, 0.059, 0.097, 0.059, 0.013 },
		{ 0.022, 0.097, 0.159, 0.097, 0.022 },
		{ 0.013, 0.059, 0.097, 0.059, 0.013 },
		{ 0.003, 0.013, 0.022, 0.013, 0.003 }
	};
	
float gWeights3x3[3][3] = {
		{ 0.0625, 0.1250, 0.0625 },
		{ 0.1250, 0.2500, 0.1250 },
		{ 0.0625, 0.1250, 0.0625 }
	};
	
// -----------------------------------------------------------------
float4 PixelShader7x7( 
	float2 UV: TEXCOORD0,
	uniform float ScaleFactor,
	uniform float Weights[7][7] ) : COLOR0
{
	float OffX = ( 1.0 / gScreenWidth ) * ScaleFactor;
	float OffY = ( 1.0 / gScreenHeight ) * ScaleFactor;
	float4 Color = ( float4 )0;
	float2 Pos;	
	for( int y = 0; y < 7; ++y )
	{
		for( int x = 0; x < 7; ++x )
		{
			Pos.x = UV.x + ( x - 3 ) * OffX;
			Pos.y = UV.y + ( y - 3 ) * OffY;
			Color += tex2D( TextureSampler, Pos ) * Weights[ x ][ y ];
		} 
	} 	
    return float4( Color.rgb, 1.0 );
}
// -----------------------------------------------------------------
float4 PixelShader5x5(  
	float2 UV: TEXCOORD0,
	uniform float ScaleFactor,
	uniform float Weights[5][5]) : COLOR0
{
	float OffX = ( 1.0 / gScreenWidth ) * ScaleFactor;
	float OffY = ( 1.0 / gScreenHeight ) * ScaleFactor;
	float4 Color = ( float4 )0;
	float2 Pos;
	for( int y = 0; y < 5; ++y )
	{
		for( int x = 0; x < 5; ++x )
		{
			Pos.x = UV.x + ( x - 2 ) * OffX;
			Pos.y = UV.y + ( y - 2 ) * OffY;
			Color += tex2D( TextureSampler, Pos ) * Weights[ x ][ y ];
		} 
	} 
	return float4( Color.rgb, 1.0 );
}
// -----------------------------------------------------------------
float4 PixelShader3x3(  
	float2 UV: TEXCOORD0,
	uniform float ScaleFactor,
	uniform float Weights[3][3]) : COLOR0
{
	float OffX = ( 1.0 / gScreenWidth ) * ScaleFactor;
	float OffY = ( 1.0 / gScreenHeight ) * ScaleFactor;
	float4 Color = ( float4 )0;
	float2 Pos;	
	for( int y = 0; y < 3; ++y )
	{
		for( int x = 0; x < 3; ++x )
		{
			Pos.x = UV.x + ( x - 1 ) * OffX;
			Pos.y = UV.y + ( y - 1 ) * OffY;
			Color += tex2D( TextureSampler, Pos ) * Weights[ x ][ y ];
		} 
	} 
	return float4( Color.rgb, 1.0 );
}
// -----------------------------------------------------------------
technique Quality7x7
{
    pass p0
    {
        VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 PixelShader7x7(
			gScaleFactor,
			gWeights7x7 );
    }
}
// -----------------------------------------------------------------
technique Quality5x5
{
    pass p0
    {
        VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 PixelShader5x5(
			gScaleFactor,
			gWeights5x5 );
    }
}
// -----------------------------------------------------------------
technique Quality3x3
{
    pass p0
    {
        VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 PixelShader3x3(
			gScaleFactor,
			gWeights3x3 );
    }
}
