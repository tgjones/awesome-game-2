const texture Texture;
const bool TextureEnabled;
const float3 DiffuseColour;
const bool IsSelected;

sampler TextureSampler = 
sampler_state
{
    Texture = <Texture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float3 GetNormal(float3 normal, float2 texCoord, float3x3 tangentToWorld)
{
	return normal;
}

float GetSpecularFactor(float2 texCoord)
{
	return 1.0f;
}

float4 GetColour(float2 texCoord, float3 nDotL)
{
	if (TextureEnabled)
		return tex2D(TextureSampler, texCoord);
	else if (IsSelected)
		return float4(DiffuseColour + float3(0.2f, 0.2f, 0.2f), 1);
	else
		return float4(DiffuseColour, 1);
}

const int CullMode;

#include "ShaderCore.fxh"