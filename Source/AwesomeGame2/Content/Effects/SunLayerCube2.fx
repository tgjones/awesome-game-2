float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldIT;

texture gTexture;

samplerCUBE gSampler = sampler_state
{
	texture = <gTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap; 
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal	: NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Normal	: TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Normal = mul( input.Normal, WorldIT );

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 Color = { 1.0f, 1.0f, 1.0f, 1.0f };
	return Color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
