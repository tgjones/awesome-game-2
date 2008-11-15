float4x4 gWorldXf;
float4x4 gViewXf;
float4x4 gProjectionXf;
float4x4 gWorldITXf;

texture gAlbedoMap;

samplerCUBE gAlbedoSampler = sampler_state
{
	texture = <gAlbedoMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap; 
};

struct VS_IN
{
    float4 Position : POSITION;
    float3 Normal	: NORMAL;
};

struct VS_OUT
{
    float4 Position : POSITION0;
	float4 Normal	: TEXCOORD0;
	float4 View		: TEXCOORD1;
};

struct PS_OUT
{
	float4 c0 : COLOR0; // albedo
	float4 c1 : COLOR1; // normal
	float4 c2 : COLOR2; // view
};
// -----------------------------------------------------------------
VS_OUT VertexShaderFunction( 
	VS_IN IN,
	uniform float4x4 WorldXf,
	uniform float4x4 ViewXf,
	uniform float4x4 ProjectionXf,
	uniform float4x4 WorldITXf )
{
    VS_OUT OUT = ( VS_OUT ) 0;

    float4 worldPosition = mul( IN.Position, WorldXf );
    float4 viewPosition = mul( worldPosition, ViewXf );
    OUT.Position = mul( viewPosition, ProjectionXf );
	OUT.Normal = mul( IN.Normal, WorldITXf );
	OUT.View = ViewXf[3] - worldPosition;

    return OUT;
}
// -----------------------------------------------------------------
PS_OUT PixelShaderFunction( 
	VS_OUT input,
	uniform samplerCUBE AlbedoMap )
{
	PS_OUT OUT = ( PS_OUT ) 0;
	float4 N = normalize( input.Normal );
	float4 V = normalize( input.View );
	float4 Color = texCUBE( AlbedoMap, N.xyz );
	OUT.c0 = float4( Color.rgb, 1.0 );
	OUT.c1 = float4( N.xyz, 1.0 );
	OUT.c2 = float4( V.xyz, 1.0 );
	return OUT;
}
// -----------------------------------------------------------------
technique Technique1
{
    pass p0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction(
			gWorldXf,
			gViewXf,
			gProjectionXf,
			gWorldITXf );
        PixelShader = compile ps_2_0 PixelShaderFunction(
			gAlbedoSampler );
    }
}
